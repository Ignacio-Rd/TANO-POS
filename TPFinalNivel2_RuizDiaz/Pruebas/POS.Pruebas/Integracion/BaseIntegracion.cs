using System;
using System.Transactions;
using NUnit.Framework;

namespace POS.Pruebas.Integracion
{
    // Cada prueba corre dentro de un TransactionScope que NUNCA se confirma (no se llama a Complete()):
    // al terminar, SQL Server deshace todo lo que la prueba escribió y la base queda como estaba.
    public abstract class BaseIntegracion : BaseConexion
    {
        private TransactionScope _transaccion;

        [SetUp]
        public void IniciarTransaccion()
        {
            _transaccion = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(2)
                });
        }

        [TearDown]
        public void RevertirTransaccion()
        {
            if (_transaccion != null)
            {
                _transaccion.Dispose();   // sin Complete() => ROLLBACK
                _transaccion = null;
            }
        }
    }
}
