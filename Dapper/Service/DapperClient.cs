using Dapper;
using Dapper.Contrib.Extensions;
using DapperLib.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DapperLib.Service
{
    public class DapperClient
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public DapperClient(IOptionsMonitor<ConnectionConfig> config)
        {
            CurrentConnectionConfig = config.CurrentValue;
        }

        public DapperClient(ConnectionConfig config) { CurrentConnectionConfig = config; }

        IDbConnection _connection = null;
        public IDbConnection Connection
        {
            get
            {
                //根据配置自动选择数据库
                switch (CurrentConnectionConfig.DbType)
                {
                    case DbStoreType.MySql:
                        _connection = new MySql.Data.MySqlClient.MySqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case DbStoreType.Sqlite:
                        _connection = new SqliteConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case DbStoreType.SqlServer:
                        _connection = new System.Data.SqlClient.SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case DbStoreType.Oracle:
                        _connection = new Oracle.ManagedDataAccess.Client.OracleConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }
        #region SQL
        #region 查

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public virtual List<T> SQuery<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, null).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual List<T> SQuery<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual T SQueryFirst<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql).FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual async Task<T> SQueryFirstAsync<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryAsync<T>(strSql);
                return res.FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual T SQueryFirst<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).FirstOrDefault<T>();
            }
        }
        #endregion
        
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns>0成功，-1执行失败</returns>
        public virtual int SExecute(string strSql, object param)
        {

            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strSql, param) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public virtual async Task<int> SExecuteAsync<T>(string strSql, T param)
        {
            Type t = typeof(T);
            foreach(PropertyInfo p in t.GetProperties())
            {
                strSql = strSql.Replace("@" + p.Name, $"{p.GetValue(param)}",StringComparison.OrdinalIgnoreCase);
                
            }
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return await conn.ExecuteAsync(strSql, param) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 执行无参存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <returns></returns>
        public virtual int SExecuteStoredProcedure(string strProcedure)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, null, null, null, CommandType.StoredProcedure) == 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行有参存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual int SExecuteStoredProcedure(string strProcedure, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, param, null, null, CommandType.StoredProcedure) == 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region Entity
        public async Task<T> EGetByIdAsync<T>(int id) where T:class
        {
            using (IDbConnection conn = Connection)
            {
                var s=await conn.GetAsync<T>(id);
                return s;
            }
        }
        #region 新增
        public async Task<int> EAddReturnId<T>(T t) where T : class
        {
            using (IDbConnection conn = Connection)
            {
                var id = await conn.InsertAsync(t);
                return id;
            }
        }
        #endregion
        #endregion


    }

}