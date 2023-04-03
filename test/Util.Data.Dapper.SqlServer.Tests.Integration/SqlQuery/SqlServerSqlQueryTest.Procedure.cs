using System;
using System.Collections;
using System.Threading.Tasks;
using Util.Data.Sql;
using Util.Helpers;
using Util.Tests.Configs;
using Util.Tests.Models;
using Xunit;

namespace Util.Data.Dapper.Tests.SqlQuery {
    /// <summary>
    /// Sql Server Sql��ѯ������� - ִ�д洢���̲���
    /// </summary>
    public partial class SqlServerSqlQueryTest {

        #region ExecuteProcedureScalar

        /// <summary>
        /// ����ִ�д洢���̻�ȡ��ֵ
        /// </summary>
        [Fact]
        public void TestExecuteProcedureScalar_1() {
            //ִ�д洢����
            var result = _sqlQuery
                .AddParam( "productId", TestConfig.Id )
                .ExecuteProcedureScalar( "Products.Proc_GetProductCode" );

            //����
            Assert.Equal( TestConfig.Value, result );
            Assert.Equal( "[Products].[Proc_GetProductCode]", _sqlQuery.PreviousSql.GetSql() );
        }

        /// <summary>
        /// ����ִ�д洢���̻�ȡ��ֵ - ���ͷ���
        /// </summary>
        [Fact]
        public void TestExecuteProcedureScalar_2() {
            //ִ�д洢����
            var result = _sqlQuery
                .AddParam( "productId", TestConfig.Id )
                .ExecuteProcedureScalar<string>( "Products.Proc_GetProductCode" );

            //����
            Assert.Equal( TestConfig.Value, result );
            Assert.Equal( "[Products].[Proc_GetProductCode]", _sqlQuery.PreviousSql.GetSql() );
        }

        #endregion

        #region ExecuteProcedureScalarAsync

        /// <summary>
        /// ����ִ�д洢���̻�ȡ��ֵ
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureScalarAsync_1() {
            //ִ�д洢����
            var result = await _sqlQuery
                .AddParam( "productId", TestConfig.Id )
                .ExecuteProcedureScalarAsync( "Products.Proc_GetProductCode" );

            //����
            Assert.Equal( TestConfig.Value, result );
            Assert.Equal( "[Products].[Proc_GetProductCode]", _sqlQuery.PreviousSql.GetSql() );
        }

        /// <summary>
        /// ����ִ�д洢���̻�ȡ��ֵ - ���ͷ���
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureScalarAsync_2() {
            //ִ�д洢����
            var result = await _sqlQuery
                .AddParam( "productId", TestConfig.Id )
                .ExecuteProcedureScalarAsync<string>( "Products.Proc_GetProductCode" );

            //����
            Assert.Equal( TestConfig.Value, result );
            Assert.Equal( "[Products].[Proc_GetProductCode]", _sqlQuery.PreviousSql.GetSql() );
        }

        #endregion

        #region ExecuteProcedureSingle

        /// <summary>
        /// ����ִ�д洢���̻�ȡ����ʵ��
        /// </summary>
        [Fact(Skip = "��չ����ת��ʧ��,��δ����")]
        public void TestExecuteProcedureSingle() {
            //ִ�д洢����
            var result = _sqlQuery
                .AddParam( "productId", TestConfig.Id )
                .ExecuteProcedureSingle<Product>( "Products.Proc_GetProduct" );

            //����
            Assert.Equal( TestConfig.Value, result.Code );
            Assert.Equal( "[Products].[Proc_GetProduct]", _sqlQuery.PreviousSql.GetSql() );
        }

        #endregion

        #region ExecuteProcedureSingleAsync

        /// <summary>
        /// ����ִ�д洢���̻�ȡ����ʵ��
        /// </summary>
        [Fact( Skip = "��չ����ת��ʧ��,��δ����" )]
        public async Task TestExecuteProcedureSingleAsync() {
            //ִ�д洢����
            var result = await _sqlQuery
                .AddParam( "productId", TestConfig.Id )
                .ExecuteProcedureSingleAsync<Product>( "Products.Proc_GetProduct" );

            //����
            Assert.Equal( TestConfig.Value, result.Code );
            Assert.Equal( "[Products].[Proc_GetProduct]", _sqlQuery.PreviousSql.GetSql() );
        }

        #endregion

        #region ExecuteProcedureQuery

        /// <summary>
        /// ����ִ�д洢���̻�ȡʵ�弯��
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureQuery_1() {
            //����2������
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            await _sqlExecutor.Insert( "ProductId,Code", "Products.Product" )
                .Values( id, "a" )
                .Values( id2, "b" )
                .ExecuteAsync();

            //ִ�д洢����
            var result = _sqlQuery
                .AddParam( "productId", id )
                .AddParam( "productId2", id2 )
                .ExecuteProcedureQuery( "Products.Proc_GetProducts" );

            //����
            Assert.Equal( 2, result.Count );
            Assert.Contains( result, t => t.Id == id && t.Code == "a" );
            Assert.Contains( result, t => t.Id == id2 && t.Code == "b" );
            Assert.Equal( "[Products].[Proc_GetProducts]", _sqlQuery.PreviousSql.GetSql() );
        }

        /// <summary>
        /// ���Ի�ȡʵ�弯�� - 1�����Ͳ���
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureQuery_2() {
            //����2������
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            await _sqlExecutor.Insert( "ProductId,Code", "Products.Product" )
                .Values( id, "a" )
                .Values( id2, "b" )
                .ExecuteAsync();

            //ִ�д洢����
            var result = _sqlQuery
                .AddParam( "productId", id )
                .AddParam( "productId2", id2 )
                .ExecuteProcedureQuery<Product>( "Products.Proc_GetProducts" );

            //����
            Assert.Equal( 2, result.Count );
            Assert.Contains( result, t => t.Id == id && t.Code == "a" );
            Assert.Contains( result, t => t.Id == id2 && t.Code == "b" );
            Assert.Equal( "[Products].[Proc_GetProducts]", _sqlQuery.PreviousSql.GetSql() );
        }

        /// <summary>
        /// ���Ի�ȡʵ�弯�� - 3�����Ͳ��� - ���ӱ�,���ù�������
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureQuery_3() {
            //���붩��
            var orderId = Id.Create();
            var customerName = "customerName";
            await _sqlExecutor.Insert( "OrderId,CustomerName", "Sales.Order" )
                .Values( orderId, customerName )
                .ExecuteAsync();

            //���붩����ϸ
            var orderItemId = Id.CreateGuid();
            await _sqlExecutor.Insert( "OrderItemId,OrderId,Price", "Sales.OrderItem" )
                .Values( orderItemId, orderId, TestConfig.DecimalValue )
                .ExecuteAsync();

            //��ȡ����
            var result = _sqlQuery
                .AddParam( "orderItemId", orderItemId )
                .ExecuteProcedureQuery<OrderItem, Order, OrderItem>( "Sales.Proc_GetOrderItem", ( item, order ) => {
                    item.Order = order;
                    return item;
                } );

            //����
            Assert.Single( (IEnumerable)result );
            Assert.Equal( orderItemId, result[0].Id );
            Assert.Equal( TestConfig.DecimalValue, result[0].Price );
            Assert.Equal( orderId, result[0].OrderId );
            Assert.Equal( orderId, result[0].Order.Id );
            Assert.Equal( customerName, result[0].Order.CustomerName );
        }

        #endregion

        #region ExecuteProcedureQueryAsync

        /// <summary>
        /// ����ִ�д洢���̻�ȡʵ�弯��
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureQueryAsync_1() {
            //����2������
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            await _sqlExecutor.Insert( "ProductId,Code", "Products.Product" )
                .Values( id, "a" )
                .Values( id2, "b" )
                .ExecuteAsync();

            //ִ�д洢����
            var result = await _sqlQuery
                .AddDynamicParams( new{ productId=id, productId2=id2 } )
                .ExecuteProcedureQueryAsync( "Products.Proc_GetProducts" );

            //����
            Assert.Equal( 2, result.Count );
            Assert.Contains( result, t => t.Id == id && t.Code == "a" );
            Assert.Contains( result, t => t.Id == id2 && t.Code == "b" );
            Assert.Equal( "[Products].[Proc_GetProducts]", _sqlQuery.PreviousSql.GetSql() );
        }

        /// <summary>
        /// ���Ի�ȡʵ�弯�� - 1�����Ͳ���
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureQueryAsync_2() {
            //����2������
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            await _sqlExecutor.Insert( "ProductId,Code", "Products.Product" )
                .Values( id, "a" )
                .Values( id2, "b" )
                .ExecuteAsync();

            //ִ�д洢����
            var result = await _sqlQuery
                .AddParam( "productId", id )
                .AddParam( "productId2", id2 )
                .ExecuteProcedureQueryAsync<Product>( "Products.Proc_GetProducts" );

            //����
            Assert.Equal( 2, result.Count );
            Assert.Contains( result, t => t.Id == id && t.Code == "a" );
            Assert.Contains( result, t => t.Id == id2 && t.Code == "b" );
            Assert.Equal( "[Products].[Proc_GetProducts]", _sqlQuery.PreviousSql.GetSql() );
        }

        /// <summary>
        /// ���Ի�ȡʵ�弯�� - 3�����Ͳ��� - ���ӱ�,���ù�������
        /// </summary>
        [Fact]
        public async Task TestExecuteProcedureQueryAsync_3() {
            //���붩��
            var orderId = Id.Create();
            var customerName = "customerName";
            await _sqlExecutor.Insert( "OrderId,CustomerName", "Sales.Order" )
                .Values( orderId, customerName )
                .ExecuteAsync();

            //���붩����ϸ
            var orderItemId = Id.CreateGuid();
            await _sqlExecutor.Insert( "OrderItemId,OrderId,Price", "Sales.OrderItem" )
                .Values( orderItemId, orderId, TestConfig.DecimalValue )
                .ExecuteAsync();

            //��ȡ����
            var result = await _sqlQuery
                .AddParam( "orderItemId", orderItemId )
                .ExecuteProcedureQueryAsync<OrderItem, Order, OrderItem>( "Sales.Proc_GetOrderItem", ( item, order ) => {
                    item.Order = order;
                    return item;
                } );

            //����
            Assert.Single( (IEnumerable)result );
            Assert.Equal( orderItemId, result[0].Id );
            Assert.Equal( TestConfig.DecimalValue, result[0].Price );
            Assert.Equal( orderId, result[0].OrderId );
            Assert.Equal( orderId, result[0].Order.Id );
            Assert.Equal( customerName, result[0].Order.CustomerName );
        }

        #endregion
    }
}