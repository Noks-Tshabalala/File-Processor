namespace FileProcessor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class X : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalculationDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        Formula = c.Int(nullable: false),
                        A = c.Int(nullable: false),
                        B = c.Int(nullable: false),
                        C = c.Int(nullable: false),
                        CalculatedResult = c.Double(),
                        ErrorMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDetails", t => t.FileId, cascadeDelete: true)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.FileDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DateProcessed = c.DateTime(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.FileName, unique: true, name: "FileNameUniqIndex");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateStoredProcedure("dbo.CalculateResult",
                s => new
                {
                    UserId = s.String(maxLength: 128),
                    FileName = s.String(maxLength: 100),
                    Formula = s.Int(),
                    A = s.Int(),
                    B = s.Int(),
                    C = s.Int()
                },
                @"Declare @FileId int = 0
				If (Select Count(FileName) From FileDetails Where FileName = @FileName) > 0 
    				Begin
    					Set @FileId = (Select Top 1 Id From FileDetails Where FileName = @FileName)
    				End
				Else 
    				Begin
    					Insert Into FileDetails(UserId,[FileName],DateProcessed)Values(@UserId,@FileName,GETDATE())
    					Select @FileId = SCOPE_IDENTITY()
    				End
    
				Declare @CalculatedResult float
				Declare @ErrMessage NVARCHAR(120)	

				If ISNUMERIC(@Formula) = 0 Begin SET @Formula = 0 End;
				If ISNUMERIC(@A) = 0 Begin SET @A = 0 End;
				If ISNUMERIC(@B) = 0 Begin SET @B = 0 End;
				If ISNUMERIC(@C) = 0 Begin SET @C = 0 End;
    
				If @Formula = 1
    	            Begin
    		            Set @CalculatedResult = CONVERT(Float,@A) * CONVERT(Float,@B) / CONVERT(Float,@C)
    	            End
    
                If @Formula = 2
    	            Begin
    		            Set @CalculatedResult = CAST((@A % @B) AS float) * CONVERT(Float,@C)
    	            End
    
                If @Formula = 3
    	            Begin
    		            Set @CalculatedResult = POWER(CONVERT(Float,@A),CONVERT(Float,@C)) - SQRT(CONVERT(Float,@B)) * CONVERT(Float,@C)
    	            End

				If @CalculatedResult IS NULL 
					Begin
						Set @ErrMessage = 'Calculation Error'
					End

				Else
					Begin
						Set @ErrMessage = Null
					End

				If (Select Count(Id) from CalculationDetails where FileId=@FileId And Formula=@Formula And A=@A And B=@B And C=@C And CalculatedResult=@CalculatedResult) > 0
					Begin					
						Insert Into CalculationDetails(FileId,Formula,A,B,C,CalculatedResult,ErrorMessage)
						Values(@FileId,@Formula,@A,@B,@C,@CalculatedResult,'Duplicate Record')
					End
				Else
					Begin
						Insert Into CalculationDetails(FileId,Formula,A,B,C,CalculatedResult,ErrorMessage)
						Values(@FileId,@Formula,@A,@B,@C,@CalculatedResult,@ErrMessage)
					End"
                );

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CalculationDetails", "FileId", "dbo.FileDetails");
            DropForeignKey("dbo.FileDetails", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.FileDetails", "FileNameUniqIndex");
            DropIndex("dbo.FileDetails", new[] { "UserId" });
            DropIndex("dbo.CalculationDetails", new[] { "FileId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.FileDetails");
            DropTable("dbo.CalculationDetails");
            DropStoredProcedure("dbo.CalculateResult");
        }
    }
}
