/*==============================================================*/
/* Database name:  ARTDB                                        */
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     2020/4/1 15:13:56                            */
/*==============================================================*/


drop database ARTDB
go

/*==============================================================*/
/* Database: ARTDB                                              */
/*==============================================================*/
create database ARTDB
go

use ARTDB
go

/*==============================================================*/
/* Table: Article                                               */
/*==============================================================*/
create table Article (
   Id                   int                  identity,
   UserId               int                  null,
   Title                nvarchar(50)         not null,
   HeadImg              varchar(128)         null,
   Content              nvarchar(Max)        null,
   Synopsis             nvarchar(300)        null,
   Author               nvarchar(50)         null,
   Section              int                  not null,
   State                int                  null default 0,
   ParCategory          int                  null,
   Category             int                  null,
   UpdateDate           datetime             null,
   CreateDate           datetime             null default getdate(),
   PublishDate          datetime             null,
   IsDeleted            bit                  null default 0,
   constraint PK_ARTICLE primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Article')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Article', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'Article', 'column', 'State'
go

/*==============================================================*/
/* Table: CategoryDictionary                                    */
/*==============================================================*/
create table CategoryDictionary (
   Id                   int                  identity,
   UserId               int                  null,
   Parent               int                  null,
   Name                 nvarchar(50)         null,
   Type                 int                  null,
   State                int                  null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_CATEGORYDICTIONARY primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('CategoryDictionary')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'CategoryDictionary', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'CategoryDictionary', 'column', 'State'
go

/*==============================================================*/
/* Table: FileDownload                                          */
/*==============================================================*/
create table FileDownload (
   Id                   int                  identity,
   UserId               int                  null,
   Name                 nvarchar(50)         null,
   Title                nvarchar(50)         not null,
   Category             int                  null,
   Type                 int                  not null,
   Src                  varchar(128)         not null,
   State                int                  null,
   Description          nvarchar(max)        null,
   Synopsis             nvarchar(200)        null,
   HeadImg              varchar(128)         null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_FILEDOWNLOAD primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('FileDownload')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'FileDownload', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'FileDownload', 'column', 'State'
go

/*==============================================================*/
/* Table: FileResource                                          */
/*==============================================================*/
create table FileResource (
   Id                   int                  identity,
   FileName             nvarchar(50)         null,
   Src                  varchar(128)         null,
   Type                 int                  null,
   SpanDays             int                  null,
   CreateDate           datetime             null default getdate(),
   DelDate              datetime             null,
   IsDeleted            bit                  null default 0,
   constraint PK_FILERESOURCE primary key (Id)
)
go

/*==============================================================*/
/* Table: MemberUnit                                            */
/*==============================================================*/
create table MemberUnit (
   Id                   int                  identity,
   UserId               int                  null,
   Name                 nvarchar(50)         not null,
   HeadImg              varchar(128)         null,
   Star                 int                  not null default 1,
   Number               varchar(50)          not null,
   State                int                  null default 0,
   Category             int                  null,
   Description          nvarchar(Max)        null,
   Synopsis             nvarchar(200)        null,
   Province             nvarchar(10)         null,
   City                 nvarchar(20)         null,
   County               nvarchar(30)         null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_MEMBERUNIT primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('MemberUnit')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'MemberUnit', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'MemberUnit', 'column', 'State'
go

/*==============================================================*/
/* Table: Organization                                          */
/*==============================================================*/
create table Organization (
   Id                   int                  identity,
   Name                 nvarchar(50)         null,
   Logo                 varchar(128)         null,
   Number               varchar(20)          null,
   Telephone            varchar(20)          null,
   Email                varchar(50)          null,
   WeChat               nvarchar(128)        null,
   Blog                 nvarchar(50)         null,
   Description          nvarchar(800)        null,
   Detail               nvarchar(Max)        null,
   Address              nvarchar(218)        null,
   Framework            nvarchar(Max)        null,
   CreateDate           datetime             null default getdate(),
   constraint PK_ORGANIZATION primary key (Id)
)
go

/*==============================================================*/
/* Table: Permission                                            */
/*==============================================================*/
create table Permission (
   Id                   int                  identity,
   Name                 nvarchar(50)         not null,
   Description          nvarchar(100)        null,
   constraint PK_PERMISSION primary key (Id)
)
go

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role (
   Id                   int                  identity,
   Name                 nvarchar(50)         not null,
   Description          nvarchar(100)        null,
   constraint PK_ROLE primary key (Id)
)
go

/*==============================================================*/
/* Table: RolePermission                                        */
/*==============================================================*/
create table RolePermission (
   Id                   int                  identity,
   RoleId               int                  not null,
   PermissionId         int                  not null,
   constraint PK_ROLEPERMISSION primary key (Id)
)
go

/*==============================================================*/
/* Table: RotationChart                                         */
/*==============================================================*/
create table RotationChart (
   Id                   int                  identity,
   ImgSrc               varchar(128)         not null,
   WebLink              varchar(300)         not null,
   State                int                  null default 0,
   Type                 int                  null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_ROTATIONCHART primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('RotationChart')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'RotationChart', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'RotationChart', 'column', 'State'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('RotationChart')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Type')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'RotationChart', 'column', 'Type'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1:轮播图
   2:logo链接',
   'user', @CurrentUser, 'table', 'RotationChart', 'column', 'Type'
go

/*==============================================================*/
/* Table: Setting                                               */
/*==============================================================*/
create table Setting (
   Id                   int                  identity,
   Type                 int                  null,
   Val                  varchar(200)         null,
   CreateDate           datetime             null default getdate(),
   IsEnabled            bit                  null default 1,
   constraint PK_SETTING primary key (Id)
)
go

/*==============================================================*/
/* Table: StuCertificate                                        */
/*==============================================================*/
create table StuCertificate (
   Id                   int                  identity,
   UserId               int                  null,
   Name                 nvarchar(20)         not null,
   HeadImg              varchar(128)         null,
   Gender               bit                  null default 1,
   Number               varchar(50)          not null,
   State                int                  null default 0,
   Category             int                  null,
   Province             nvarchar(10)         null,
   City                 nvarchar(20)         null,
   County               nvarchar(30)         null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_STUCERTIFICATE primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('StuCertificate')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'StuCertificate', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'StuCertificate', 'column', 'State'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('StuCertificate')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Category')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'StuCertificate', 'column', 'Category'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '固定类别:培训、获奖',
   'user', @CurrentUser, 'table', 'StuCertificate', 'column', 'Category'
go

/*==============================================================*/
/* Table: SystemLog                                             */
/*==============================================================*/
create table SystemLog (
   Id                   int                  identity,
   UserId               int                  null,
   Type                 int                  null,
   Source               int                  null,
   ControllerName       varchar(100)         null,
   ActionName           varchar(100)         null,
   ClientIp             varchar(50)          null,
   UserAgent            varchar(500)         null,
   ReqParameter         varchar(1000)        null,
   ResultLog            varchar(6000)        null,
   CreateDate           datetime             null default getdate(),
   constraint PK_SYSTEMLOG primary key (Id)
)
go

/*==============================================================*/
/* Table: TeaCertificate                                        */
/*==============================================================*/
create table TeaCertificate (
   Id                   int                  identity,
   UserId               int                  null,
   Name                 nvarchar(20)         not null,
   Gender               bit                  null default 1,
   HeadImg              varchar(128)         null,
   Number               varchar(50)          not null,
   State                int                  null default 0,
   Category             int                  null,
   Level                int                  null,
   Province             nvarchar(10)         null,
   City                 nvarchar(20)         null,
   County               nvarchar(30)         null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_TEACERTIFICATE primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('TeaCertificate')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'TeaCertificate', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'TeaCertificate', 'column', 'State'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('TeaCertificate')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Category')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'TeaCertificate', 'column', 'Category'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '固定类别:培训、获奖',
   'user', @CurrentUser, 'table', 'TeaCertificate', 'column', 'Category'
go

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table "User" (
   Id                   int                  identity,
   UserId               int                  null,
   UserName             varchar(50)          not null,
   Password             varchar(50)          not null,
   Salt                 varchar(20)          null,
   Name                 nvarchar(50)         null,
   Email                varchar(50)          null,
   Telephone            varchar(20)          null,
   HeadImg              varchar(128)         null,
   IsAdmin              bit                  null default 0,
   State                int                  null,
   CreateDate           datetime             null default getdate(),
   IsDeleted            bit                  null default 0,
   constraint PK_USER primary key (Id)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('"User"')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'User', 'column', 'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:下架
   1:上架',
   'user', @CurrentUser, 'table', 'User', 'column', 'State'
go

/*==============================================================*/
/* Table: UserRole                                              */
/*==============================================================*/
create table UserRole (
   Id                   int                  identity,
   UserId               int                  not null,
   RoleId               int                  not null,
   constraint PK_USERROLE primary key (Id)
)
go

alter table RolePermission
   add constraint FK_ROLEPERM_REFERENCE_PERMISSI foreign key (PermissionId)
      references Permission (Id)
go

alter table RolePermission
   add constraint FK_ROLEPERM_REFERENCE_ROLE foreign key (RoleId)
      references Role (Id)
go

alter table UserRole
   add constraint FK_USERROLE_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table UserRole
   add constraint FK_USERROLE_REFERENCE_ROLE foreign key (RoleId)
      references Role (Id)
go

