using System;
namespace Authorization.Data
{
    public class UsersData
    { 
    public int 		    ID		        { get; set; } 
    public int 		    UserId		    { get; set; } 
    public string 		UserName		{ get; set; } 
    public string 		FirstName	    { get; set; } 
    public string 		MiddleName		{ get; set; } 
    public string 		LastName		{ get; set; } 
    public string 		Company		    { get; set; } 
    public string 		Email		    { get; set; } 
    public string 		Phone		    { get; set; } 
    public DateTime 	CreatedOn		{ get; set; } 
    public long 		ModuleBit		{ get; set; } 
    public long		    RolesBit		{ get; set; } 
    public bool 		Archive		    { get; set; } 
     
    }
}