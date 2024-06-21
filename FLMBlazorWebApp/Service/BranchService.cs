using Dapper;
using FLMBlazorWebApp.Components.Pages;
using FLMBlazorWebApp.Interface;
using Model = FLMBlazorWebApp.Repositories;
using Microsoft.AspNetCore.Components;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FLMBlazorWebApp.Service
{
    public class BranchService : IBranchService
    {
        List<Model.Branch> _branches = new List<Model.Branch>();
        Model.Branch _branch = new Model.Branch();
        int oldId { get; set; }

        public IConfiguration _Configuration { get; }
        public string _connectionString = "";

        public BranchService(IConfiguration configuration)
        {
            _Configuration = configuration;
            _connectionString = _Configuration.GetConnectionString("FLMDbConnection");
        }

        public List<Model.Branch> GetBranches()
        {
            _branches = new List<Model.Branch>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var branches = connection.Query<Model.Branch>("SELECT * FROM Branch").ToList();

                if (branches != null && branches.Count() > 0)
                {
                    _branches = branches;
                }
            }
            return _branches;
        }

        public string Delete(int branchId)
        {
            string message = "Failed";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                _branch = new Model.Branch()
                {
                    Id = branchId
                };

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                // var branches = connection.Query<Branch>("",
                // this.SetParameters(_branch, (int)OperationType.Delete),
                // commandType: CommandType.StoredProcedure);

                var branches = connection.Query<Model.Branch>("DELETE FROM Branch WHERE Id=@BranchId", new {branchId});

                message = "Deleted";
            }
            return message;
        }

        public Model.Branch GetById(int branchId)
        {
            oldId = branchId;
            _branch = new Model.Branch();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var branches = connection.Query<Model.Branch>("SELECT * FROM Branch WHERE ID = " + branchId);

                if (branches != null && branches.Count() > 0)
                {
                    _branch = branches.FirstOrDefault();
                }
            }
            return _branch;
        }

        public Model.Branch SaveOrUpdate(Model.Branch branch)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                _branch = new Model.Branch()
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    TelephoneNumber = branch.TelephoneNumber,
                    OpenDate = branch.OpenDate
                };

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                
                int operationType = Convert.ToInt32(oldId == 0 ? Model.OperationType.Insert : Model.OperationType.Update);
                // int operationType = Convert.ToInt32(branch.Id == 0 ? Model.OperationType.Insert : Model.OperationType.Update);

                IEnumerable<Model.Branch> branches;

                if (operationType == (int)Model.OperationType.Insert)
                {
                    branches = connection.Query<Model.Branch>("INSERT INTO Branch(Id, Name, TelephoneNumber, OpenDate) VALUES(@BranchId, @Name, @TelephoneNumber, @OpenDate)", new { @BranchId = branch.Id, @Name = branch.Name, @TelephoneNumber = branch.TelephoneNumber, @OpenDate = branch.OpenDate });
                }
                else
                {
                    branches = connection.Query<Model.Branch>("UPDATE Branch SET Id = @BranchId, Name = @Name, TelephoneNumber = @TelephoneNumber, OpenDate = @OpenDate WHERE Id = @Id", new { @BranchId = branch.Id, @Name = branch.Name, @TelephoneNumber = branch.TelephoneNumber, @OpenDate = branch.OpenDate, @Id = oldId });
                }

                if (branches != null && branches.Count() > 0)
                {
                    _branch = branches.FirstOrDefault();
                }
            }
            return _branch;
        }

        private DynamicParameters SetParameters(Model.Branch branch, int operationType)
        {
            DynamicParameters obj = new DynamicParameters();
            obj.Add("@BranchId", branch.Id);
            obj.Add("@Name", branch.Name);
            obj.Add("@TelephoneNumber", branch.TelephoneNumber);
            obj.Add("@OpenDate", branch.OpenDate);
            obj.Add("@OperationType", operationType);

            return obj;
        }
    }
}
