#region Auto-generated classes for main database on 2010-05-05 21:45:38Z

//
//  ____  _     __  __      _        _
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from main on 2010-05-05 21:45:38Z
// Please visit http://linq.to/db for more information

#endregion

using System;
using System.Data;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Reflection;
using DbLinq.Data.Linq;
using DbLinq.Vendor;
using System.ComponentModel;

namespace WilliamsonFamily.Models.Data
{
	public partial class Main : DataContext
	{
		public Main(IDbConnection connection)
		: base(connection, new DbLinq.Sqlite.SqliteVendor())
		{
		}

		public Main(IDbConnection connection, IVendor vendor)
		: base(connection, vendor)
		{
		}

		public Table<Blog> Blog { get { return GetTable<Blog>(); } }
		public Table<BlogComment> BlogComment { get { return GetTable<BlogComment>(); } }
		public Table<Family> Family { get { return GetTable<Family>(); } }
		public Table<UserFamily> UserFamily { get { return GetTable<UserFamily>(); } }
		public Table<User> User { get { return GetTable<User>(); } }

	}

	[Table(Name = "main.Blog")]
	public partial class Blog : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region string AuthorID

		private string _authorID;
		[DebuggerNonUserCode]
		[Column(Storage = "_authorID", Name = "AuthorID", DbType = "varchar(36)", CanBeNull = false)]
		public string AuthorID
		{
			get
			{
				return _authorID;
			}
			set
			{
				if (value != _authorID)
				{
					_authorID = value;
					OnPropertyChanged("AuthorID");
				}
			}
		}

		#endregion

		#region DateTime? DatePublished

		private DateTime? _datePublished;
		[DebuggerNonUserCode]
		[Column(Storage = "_datePublished", Name = "DatePublished", DbType = "TIMESTAMP")]
		public DateTime? DatePublished
		{
			get
			{
				return _datePublished;
			}
			set
			{
				if (value != _datePublished)
				{
					_datePublished = value;
					OnPropertyChanged("DatePublished");
				}
			}
		}

		#endregion

		#region string Entry

		private string _entry;
		[DebuggerNonUserCode]
		[Column(Storage = "_entry", Name = "Entry", DbType = "TEXT")]
		public string Entry
		{
			get
			{
				return _entry;
			}
			set
			{
				if (value != _entry)
				{
					_entry = value;
					OnPropertyChanged("Entry");
				}
			}
		}

		#endregion

		#region int PkID

		private int _pkID;
		[DebuggerNonUserCode]
		[Column(Storage = "_pkID", Name = "PkID", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int PkID
		{
			get
			{
				return _pkID;
			}
			set
			{
				if (value != _pkID)
				{
					_pkID = value;
					OnPropertyChanged("PkID");
				}
			}
		}

		#endregion

		#region string Tags

		private string _tags;
		[DebuggerNonUserCode]
		[Column(Storage = "_tags", Name = "Tags", DbType = "VARCHAR(50)")]
		public string Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				if (value != _tags)
				{
					_tags = value;
					OnPropertyChanged("Tags");
				}
			}
		}

		#endregion

		#region string Title

		private string _title;
		[DebuggerNonUserCode]
		[Column(Storage = "_title", Name = "Title", DbType = "VARCHAR(50)", CanBeNull = false)]
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (value != _title)
				{
					_title = value;
					OnPropertyChanged("Title");
				}
			}
		}

		#endregion

		#region string Slug

		private string _slug;
		[DebuggerNonUserCode]
		[Column(Storage = "_slug", Name = "Slug", DbType = "VARCHAR(50)", CanBeNull = false)]
		public string Slug
		{
			get
			{
				return _slug;
			}
			set
			{
				if (value != _slug)
				{
					_slug = value;
					OnPropertyChanged("Slug");
				}
			}
		}

		#endregion

		#region string AuthorName

		private string _authorName;
		[DebuggerNonUserCode]
		[Column(Storage = "_authorName", Name = "AuthorName", DbType = "VARCHAR(50)")]
		public string AuthorName
		{
			get
			{
				return _authorName;
			}
			set
			{
				if (value != _authorName)
				{
					_authorName = value;
					OnPropertyChanged("AuthorName");
				}
			}
		}

		#endregion

		#region bool IsPublished

		private bool _isPublished;
		[DebuggerNonUserCode]
		[Column(Storage = "_isPublished", Name = "IsPublished", DbType = "BOOLEAN", CanBeNull = false)]
		public bool IsPublished
		{
			get
			{
				return _isPublished;
			}
			set
			{
				if (value != _isPublished)
				{
					_isPublished = value;
					OnPropertyChanged("IsPublished");
				}
			}
		}

		#endregion

		#region ctor

		public Blog()
		{
		}

		#endregion

	}

	[Table(Name = "main.BlogComment")]
	public partial class BlogComment : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region string AuthorID

		private string _authorID;
		[DebuggerNonUserCode]
		[Column(Storage = "_authorID", Name = "AuthorID", DbType = "varchar(36)", CanBeNull = false)]
		public string AuthorID
		{
			get
			{
				return _authorID;
			}
			set
			{
				if (value != _authorID)
				{
					_authorID = value;
					OnPropertyChanged("AuthorID");
				}
			}
		}

		#endregion

		#region int BlogID

		private int _blogID;
		[DebuggerNonUserCode]
		[Column(Storage = "_blogID", Name = "BlogID", DbType = "INTEGER", CanBeNull = false)]
		public int BlogID
		{
			get
			{
				return _blogID;
			}
			set
			{
				if (value != _blogID)
				{
					_blogID = value;
					OnPropertyChanged("BlogID");
				}
			}
		}

		#endregion

		#region string Comment

		private string _comment;
		[DebuggerNonUserCode]
		[Column(Storage = "_comment", Name = "Comment", DbType = "TEXT", CanBeNull = false)]
		public string Comment
		{
			get
			{
				return _comment;
			}
			set
			{
				if (value != _comment)
				{
					_comment = value;
					OnPropertyChanged("Comment");
				}
			}
		}

		#endregion

		#region DateTime? DatePublished

		private DateTime? _datePublished;
		[DebuggerNonUserCode]
		[Column(Storage = "_datePublished", Name = "DatePublished", DbType = "TIMESTAMP")]
		public DateTime? DatePublished
		{
			get
			{
				return _datePublished;
			}
			set
			{
				if (value != _datePublished)
				{
					_datePublished = value;
					OnPropertyChanged("DatePublished");
				}
			}
		}

		#endregion

		#region int PkID

		private int _pkID;
		[DebuggerNonUserCode]
		[Column(Storage = "_pkID", Name = "PkID", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int PkID
		{
			get
			{
				return _pkID;
			}
			set
			{
				if (value != _pkID)
				{
					_pkID = value;
					OnPropertyChanged("PkID");
				}
			}
		}

		#endregion

		#region ctor

		public BlogComment()
		{
		}

		#endregion

	}

	[Table(Name = "main.Family")]
	public partial class Family : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region string Description

		private string _description;
		[DebuggerNonUserCode]
		[Column(Storage = "_description", Name = "Description", DbType = "TEXT")]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (value != _description)
				{
					_description = value;
					OnPropertyChanged("Description");
				}
			}
		}

		#endregion

		#region string FamilyName

		private string _familyName;
		[DebuggerNonUserCode]
		[Column(Storage = "_familyName", Name = "FamilyName", DbType = "VARCHAR(20)", CanBeNull = false)]
		public string FamilyName
		{
			get
			{
				return _familyName;
			}
			set
			{
				if (value != _familyName)
				{
					_familyName = value;
					OnPropertyChanged("FamilyName");
				}
			}
		}

		#endregion

		#region int PkID

		private int _pkID;
		[DebuggerNonUserCode]
		[Column(Storage = "_pkID", Name = "PkID", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int PkID
		{
			get
			{
				return _pkID;
			}
			set
			{
				if (value != _pkID)
				{
					_pkID = value;
					OnPropertyChanged("PkID");
				}
			}
		}

		#endregion

		#region ctor

		public Family()
		{
		}

		#endregion

	}

	[Table(Name = "main.UserFamily")]
	public partial class UserFamily : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region int? FamilyID

		private int? _familyID;
		[DebuggerNonUserCode]
		[Column(Storage = "_familyID", Name = "FamilyID", DbType = "INTEGER")]
		public int? FamilyID
		{
			get
			{
				return _familyID;
			}
			set
			{
				if (value != _familyID)
				{
					_familyID = value;
					OnPropertyChanged("FamilyID");
				}
			}
		}

		#endregion

		#region string UserID

		private string _userID;
		[DebuggerNonUserCode]
		[Column(Storage = "_userID", Name = "UserID", DbType = "varchar(36)")]
		public string UserID
		{
			get
			{
				return _userID;
			}
			set
			{
				if (value != _userID)
				{
					_userID = value;
					OnPropertyChanged("UserID");
				}
			}
		}

		#endregion

		#region ctor

		public UserFamily()
		{
		}

		#endregion

	}

	[Table(Name = "main.users")]
	public partial class User : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region string ApplicationName

		private string _applicationName;
		[DebuggerNonUserCode]
		[Column(Storage = "_applicationName", Name = "ApplicationName", DbType = "varchar(100)", CanBeNull = false)]
		public string ApplicationName
		{
			get
			{
				return _applicationName;
			}
			set
			{
				if (value != _applicationName)
				{
					_applicationName = value;
					OnPropertyChanged("ApplicationName");
				}
			}
		}

		#endregion

		#region DateTime? Birthdate

		private DateTime? _birthdate;
		[DebuggerNonUserCode]
		[Column(Storage = "_birthdate", Name = "Birthdate", DbType = "DATE")]
		public DateTime? Birthdate
		{
			get
			{
				return _birthdate;
			}
			set
			{
				if (value != _birthdate)
				{
					_birthdate = value;
					OnPropertyChanged("Birthdate");
				}
			}
		}

		#endregion

		#region string Comment

		private string _comment;
		[DebuggerNonUserCode]
		[Column(Storage = "_comment", Name = "Comment", DbType = "varchar(255)")]
		public string Comment
		{
			get
			{
				return _comment;
			}
			set
			{
				if (value != _comment)
				{
					_comment = value;
					OnPropertyChanged("Comment");
				}
			}
		}

		#endregion

		#region DateTime? CreationDate

		private DateTime? _creationDate;
		[DebuggerNonUserCode]
		[Column(Storage = "_creationDate", Name = "CreationDate", DbType = "datetime")]
		public DateTime? CreationDate
		{
			get
			{
				return _creationDate;
			}
			set
			{
				if (value != _creationDate)
				{
					_creationDate = value;
					OnPropertyChanged("CreationDate");
				}
			}
		}

		#endregion

		#region string Email

		private string _email;
		[DebuggerNonUserCode]
		[Column(Storage = "_email", Name = "Email", DbType = "varchar(100)", CanBeNull = false)]
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				if (value != _email)
				{
					_email = value;
					OnPropertyChanged("Email");
				}
			}
		}

		#endregion

		#region int? FailedPasswordAnswerAttemptCount

		private int? _failedPasswordAnswerAttemptCount;
		[DebuggerNonUserCode]
		[Column(Storage = "_failedPasswordAnswerAttemptCount", Name = "FailedPasswordAnswerAttemptCount", DbType = "int(11)")]
		public int? FailedPasswordAnswerAttemptCount
		{
			get
			{
				return _failedPasswordAnswerAttemptCount;
			}
			set
			{
				if (value != _failedPasswordAnswerAttemptCount)
				{
					_failedPasswordAnswerAttemptCount = value;
					OnPropertyChanged("FailedPasswordAnswerAttemptCount");
				}
			}
		}

		#endregion

		#region DateTime? FailedPasswordAnswerAttemptWindowStart

		private DateTime? _failedPasswordAnswerAttemptWindowStart;
		[DebuggerNonUserCode]
		[Column(Storage = "_failedPasswordAnswerAttemptWindowStart", Name = "FailedPasswordAnswerAttemptWindowStart", DbType = "datetime")]
		public DateTime? FailedPasswordAnswerAttemptWindowStart
		{
			get
			{
				return _failedPasswordAnswerAttemptWindowStart;
			}
			set
			{
				if (value != _failedPasswordAnswerAttemptWindowStart)
				{
					_failedPasswordAnswerAttemptWindowStart = value;
					OnPropertyChanged("FailedPasswordAnswerAttemptWindowStart");
				}
			}
		}

		#endregion

		#region int? FailedPasswordAttemptCount

		private int? _failedPasswordAttemptCount;
		[DebuggerNonUserCode]
		[Column(Storage = "_failedPasswordAttemptCount", Name = "FailedPasswordAttemptCount", DbType = "int(11)")]
		public int? FailedPasswordAttemptCount
		{
			get
			{
				return _failedPasswordAttemptCount;
			}
			set
			{
				if (value != _failedPasswordAttemptCount)
				{
					_failedPasswordAttemptCount = value;
					OnPropertyChanged("FailedPasswordAttemptCount");
				}
			}
		}

		#endregion

		#region DateTime? FailedPasswordAttemptWindowStart

		private DateTime? _failedPasswordAttemptWindowStart;
		[DebuggerNonUserCode]
		[Column(Storage = "_failedPasswordAttemptWindowStart", Name = "FailedPasswordAttemptWindowStart", DbType = "datetime")]
		public DateTime? FailedPasswordAttemptWindowStart
		{
			get
			{
				return _failedPasswordAttemptWindowStart;
			}
			set
			{
				if (value != _failedPasswordAttemptWindowStart)
				{
					_failedPasswordAttemptWindowStart = value;
					OnPropertyChanged("FailedPasswordAttemptWindowStart");
				}
			}
		}

		#endregion

		#region string FirstName

		private string _firstName;
		[DebuggerNonUserCode]
		[Column(Storage = "_firstName", Name = "FirstName", DbType = "VARCHAR(50)")]
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				if (value != _firstName)
				{
					_firstName = value;
					OnPropertyChanged("FirstName");
				}
			}
		}

		#endregion

		#region bool? IsApproved

		private bool? _isApproved;
		[DebuggerNonUserCode]
		[Column(Storage = "_isApproved", Name = "IsApproved", DbType = "tinyint(1)")]
		public bool? IsApproved
		{
			get
			{
				return _isApproved;
			}
			set
			{
				if (value != _isApproved)
				{
					_isApproved = value;
					OnPropertyChanged("IsApproved");
				}
			}
		}

		#endregion

		#region bool? IsLockedOut

		private bool? _isLockedOut;
		[DebuggerNonUserCode]
		[Column(Storage = "_isLockedOut", Name = "IsLockedOut", DbType = "tinyint(1)")]
		public bool? IsLockedOut
		{
			get
			{
				return _isLockedOut;
			}
			set
			{
				if (value != _isLockedOut)
				{
					_isLockedOut = value;
					OnPropertyChanged("IsLockedOut");
				}
			}
		}

		#endregion

		#region bool? IsOnLine

		private bool? _isOnLine;
		[DebuggerNonUserCode]
		[Column(Storage = "_isOnLine", Name = "IsOnLine", DbType = "tinyint(1)")]
		public bool? IsOnLine
		{
			get
			{
				return _isOnLine;
			}
			set
			{
				if (value != _isOnLine)
				{
					_isOnLine = value;
					OnPropertyChanged("IsOnLine");
				}
			}
		}

		#endregion

		#region DateTime? LastActivityDate

		private DateTime? _lastActivityDate;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastActivityDate", Name = "LastActivityDate", DbType = "datetime")]
		public DateTime? LastActivityDate
		{
			get
			{
				return _lastActivityDate;
			}
			set
			{
				if (value != _lastActivityDate)
				{
					_lastActivityDate = value;
					OnPropertyChanged("LastActivityDate");
				}
			}
		}

		#endregion

		#region DateTime? LastLockedOutDate

		private DateTime? _lastLockedOutDate;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastLockedOutDate", Name = "LastLockedOutDate", DbType = "datetime")]
		public DateTime? LastLockedOutDate
		{
			get
			{
				return _lastLockedOutDate;
			}
			set
			{
				if (value != _lastLockedOutDate)
				{
					_lastLockedOutDate = value;
					OnPropertyChanged("LastLockedOutDate");
				}
			}
		}

		#endregion

		#region DateTime? LastLoginDate

		private DateTime? _lastLoginDate;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastLoginDate", Name = "LastLoginDate", DbType = "datetime")]
		public DateTime? LastLoginDate
		{
			get
			{
				return _lastLoginDate;
			}
			set
			{
				if (value != _lastLoginDate)
				{
					_lastLoginDate = value;
					OnPropertyChanged("LastLoginDate");
				}
			}
		}

		#endregion

		#region string LastName

		private string _lastName;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastName", Name = "LastName", DbType = "VARCHAR(50)")]
		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				if (value != _lastName)
				{
					_lastName = value;
					OnPropertyChanged("LastName");
				}
			}
		}

		#endregion

		#region DateTime? LastPasswordChangedDate

		private DateTime? _lastPasswordChangedDate;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastPasswordChangedDate", Name = "LastPasswordChangedDate", DbType = "datetime")]
		public DateTime? LastPasswordChangedDate
		{
			get
			{
				return _lastPasswordChangedDate;
			}
			set
			{
				if (value != _lastPasswordChangedDate)
				{
					_lastPasswordChangedDate = value;
					OnPropertyChanged("LastPasswordChangedDate");
				}
			}
		}

		#endregion

		#region string Password

		private string _password;
		[DebuggerNonUserCode]
		[Column(Storage = "_password", Name = "Password", DbType = "varchar(128)", CanBeNull = false)]
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				if (value != _password)
				{
					_password = value;
					OnPropertyChanged("Password");
				}
			}
		}

		#endregion

		#region string PasswordAnswer

		private string _passwordAnswer;
		[DebuggerNonUserCode]
		[Column(Storage = "_passwordAnswer", Name = "PasswordAnswer", DbType = "varchar(255)")]
		public string PasswordAnswer
		{
			get
			{
				return _passwordAnswer;
			}
			set
			{
				if (value != _passwordAnswer)
				{
					_passwordAnswer = value;
					OnPropertyChanged("PasswordAnswer");
				}
			}
		}

		#endregion

		#region string PasswordQuestion

		private string _passwordQuestion;
		[DebuggerNonUserCode]
		[Column(Storage = "_passwordQuestion", Name = "PasswordQuestion", DbType = "varchar(255)")]
		public string PasswordQuestion
		{
			get
			{
				return _passwordQuestion;
			}
			set
			{
				if (value != _passwordQuestion)
				{
					_passwordQuestion = value;
					OnPropertyChanged("PasswordQuestion");
				}
			}
		}

		#endregion

		#region string PkID

		private string _pkID;
		[DebuggerNonUserCode]
		[Column(Storage = "_pkID", Name = "PKID", DbType = "varchar(36)", CanBeNull = false)]
		public string PkID
		{
			get
			{
				return _pkID;
			}
			set
			{
				if (value != _pkID)
				{
					_pkID = value;
					OnPropertyChanged("PkID");
				}
			}
		}

		#endregion

		#region string Username

		private string _username;
		[DebuggerNonUserCode]
		[Column(Storage = "_username", Name = "Username", DbType = "varchar(255)", CanBeNull = false)]
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				if (value != _username)
				{
					_username = value;
					OnPropertyChanged("Username");
				}
			}
		}

		#endregion

		#region ctor

		public User()
		{
		}

		#endregion

	}
}
