use master
create database HomeeDB
use HomeeDB
CREATE TABLE [dbo].[Category](
	[CategoryId] int IDENTITY(1,1) NOT NULL,
	[CategoryName] nvarchar(250) NOT NULL
	);
	CREATE TABLE [dbo].[CategoryPlace](
	[CategoryId] int NOT NULL,
	[PlaceId] int NOT NULL,
	    CONSTRAINT FK_CategoryPlace_Place FOREIGN KEY (PlaceId) REFERENCES Place(PlaceId),
		    CONSTRAINT FK_CategoryPlace_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
			);
-- Create Account table
CREATE TABLE Account (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    ImageUrl NVARCHAR(255) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    CitizenId NVARCHAR(50) NOT NULL,
    Role INT NOT NULL,
    BirthDay DATE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
	GoogleUserId NVARCHAR(255) NULL,
	IsGoogleAuthenticated BIT NOT NULL DEFAULT 0,
    IsBlock BIT NOT NULL
);

-- Create Contract table
CREATE TABLE Contract (
    ContractId INT PRIMARY KEY IDENTITY(1,1),
    RenderId INT NOT NULL,
    PlaceId INT NOT NULL,
    Duration TIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    CONSTRAINT FK_Contract_Render FOREIGN KEY (RenderId) REFERENCES Account(AccountId),
    CONSTRAINT FK_Contract_Place FOREIGN KEY (PlaceId) REFERENCES Place(PlaceId)
);

-- Create Conversation table
CREATE TABLE Conversation (
    ConversationId INT PRIMARY KEY IDENTITY(1,1),
    CreatedAt DATETIME NOT NULL,
    LastMessageId INT NOT NULL
);

-- Create ConversationParticipant table
CREATE TABLE ConversationParticipant (
    AccountId INT NOT NULL,
    ConversationId INT NOT NULL,
    PRIMARY KEY (AccountId, ConversationId),
    CONSTRAINT FK_ConversationParticipant_Account FOREIGN KEY (AccountId) REFERENCES Account(AccountId),
    CONSTRAINT FK_ConversationParticipant_Conversation FOREIGN KEY (ConversationId) REFERENCES Conversation(ConversationId)
);

-- Create FavoritePost table
CREATE TABLE FavoritePost (
    AccountId INT NOT NULL,
    PostId INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    PRIMARY KEY (AccountId, PostId),
    CONSTRAINT FK_FavoritePost_Account FOREIGN KEY (AccountId) REFERENCES Account(AccountId),
    CONSTRAINT FK_FavoritePost_Post FOREIGN KEY (PostId) REFERENCES Post(PostId)
);

-- Create Image table
CREATE TABLE Image (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    ImageUrl NVARCHAR(255) NOT NULL,
    PostId INT NOT NULL,
    CONSTRAINT FK_Image_Post FOREIGN KEY (PostId) REFERENCES Post(PostId)
);

-- Create Interior table
CREATE TABLE Interior (
    InteriorId INT PRIMARY KEY IDENTITY(1,1),
    InteriorName NVARCHAR(255) NOT NULL,
    Status INT NOT NULL,
    Description NVARCHAR(MAX),
    PlaceId INT NOT NULL,
    CONSTRAINT FK_Interior_Place FOREIGN KEY (PlaceId) REFERENCES Place(PlaceId)
);

-- Create Message table
CREATE TABLE Message (
    MessageId INT PRIMARY KEY IDENTITY(1,1),
    ConversationId INT NOT NULL,
    SenderId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Timestamp DATETIME NOT NULL,
    CONSTRAINT FK_Message_Sender FOREIGN KEY (SenderId) REFERENCES Account(AccountId),
    CONSTRAINT FK_Message_Conversation FOREIGN KEY (ConversationId) REFERENCES Conversation(ConversationId)
);

-- Create Notification table
CREATE TABLE Notification (
    NotificationId INT PRIMARY KEY IDENTITY(1,1),
    AccountId INT NOT NULL,
    Type INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    UrlPath NVARCHAR(255),
    CreatedAt DATETIME NOT NULL,
    CONSTRAINT FK_Notification_Account FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);

-- Create Order table
CREATE TABLE [Order] (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    SubscriptionId INT NOT NULL,
    OwnerId INT NOT NULL,
    ExpiredAt DATETIME NOT NULL,
    SubscribedAt DATETIME NOT NULL,
    CONSTRAINT FK_Order_Owner FOREIGN KEY (OwnerId) REFERENCES Account(AccountId),
    CONSTRAINT FK_Order_Subscription FOREIGN KEY (SubscriptionId) REFERENCES Subscription(SubscriptionId)
);

-- Create Place table
CREATE TABLE Place (
    PlaceId INT PRIMARY KEY IDENTITY(1,1),
    Province NVARCHAR(255) NOT NULL,
    [Distinct] NVARCHAR(255) NOT NULL,
    Ward NVARCHAR(255) NOT NULL,
    Street NVARCHAR(255) NOT NULL,
    Number NVARCHAR(50) NOT NULL,
    Area FLOAT NOT NULL,
    Direction INT NOT NULL,
    NumberOfToilet INT NOT NULL,
    NumberOfBedroom INT NOT NULL,
    Rent FLOAT NOT NULL,
    OwnerId INT NOT NULL,
    Status INT NOT NULL,
    CONSTRAINT FK_Place_Owner FOREIGN KEY (OwnerId) REFERENCES Account(AccountId)
);

-- Create Post table
CREATE TABLE Post (
    PostId INT PRIMARY KEY IDENTITY(1,1),
    PostedDate DATETIME NOT NULL,
    Note NVARCHAR(MAX),
    PlaceId INT NOT NULL,
    Status INT NOT NULL,
    IsBlock BIT NOT NULL,
    StaffId INT,
    CONSTRAINT FK_Post_Place FOREIGN KEY (PlaceId) REFERENCES Place(PlaceId),
    CONSTRAINT FK_Post_Staff FOREIGN KEY (StaffId) REFERENCES Account(AccountId)
);

-- Create Subscription table
CREATE TABLE Subscription (
    SubscriptionId INT PRIMARY KEY IDENTITY(1,1),
    Price FLOAT NOT NULL,
    Duration TIME NOT NULL,
    SubscriptionName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX)
);




-- Insert dummy data into Subscription table
INSERT INTO Subscription (Price, Duration, SubscriptionName, Description) VALUES 
(9.99, '00:30:00', 'Basic Plan', 'Basic subscription plan'),
(19.99, '01:00:00', 'Standard Plan', 'Standard subscription plan'),
(29.99, '01:30:00', 'Premium Plan', 'Premium subscription plan');

-- Insert dummy data into Account table
INSERT INTO Account (Email, Password, ImageUrl, Name, Phone, CitizenId, Role, BirthDay, CreatedAt, UpdatedAt, IsBlock) VALUES 
('john.doe@example.com', 'password123', 'https://example.com/image1.jpg', 'John Doe', '1234567890', 'CID12345', 1, '1985-05-15', GETDATE(), GETDATE(), 0),
('jane.smith@example.com', 'password456', 'https://example.com/image2.jpg', 'Jane Smith', '0987654321', 'CID67890', 2, '1990-12-10', GETDATE(), GETDATE(), 0);

-- Insert dummy data into Place table
INSERT INTO Place (Province, [Distinct], Ward, Street, Number, Area, Direction, NumberOfToilet, NumberOfBedroom, Rent, OwnerId, [Status]) VALUES 
('Province1', 'Distinct1', 'Ward1', 'Street1', '101', 1200, 1, 2, 3, 1500, 1, 1),
('Province2', 'Distinct2', 'Ward2', 'Street2', '202', 800, 2, 1, 2, 1000, 2, 1);

-- Insert dummy data into Contract table
INSERT INTO Contract (RenderId, PlaceId, Duration, CreatedAt) VALUES 
(1, 1, '00:45:00', GETDATE()),
(2, 2, '01:15:00', GETDATE());

-- Insert dummy data into Conversation table
INSERT INTO Conversation (CreatedAt, LastMessageId) VALUES 
(GETDATE(), 1),
(GETDATE(), 2);

-- Insert dummy data into ConversationParticipant table
INSERT INTO ConversationParticipant (AccountId, ConversationId) VALUES 
(1, 1),
(2, 1),
(1, 2);

-- Insert dummy data into Message table
INSERT INTO Message (ConversationId, SenderId, Content, Timestamp) VALUES 
(1, 1, 'Hello, how are you?', GETDATE()),
(1, 2, 'I am fine, thank you!', GETDATE()),
(2, 1, 'Are you available tomorrow?', GETDATE());

-- Insert dummy data into Notification table
INSERT INTO Notification (AccountId, Type, Content, UrlPath, CreatedAt) VALUES 
(1, 1, 'Your subscription is about to expire', '/subscriptions', GETDATE()),
(2, 2, 'New message received', '/messages', GETDATE());

-- Insert dummy data into Order table
INSERT INTO [Order] (SubscriptionId, OwnerId, ExpiredAt, SubscribedAt) VALUES 
(1, 1, DATEADD(MONTH, 1, GETDATE()), GETDATE()),
(2, 2, DATEADD(MONTH, 1, GETDATE()), GETDATE());

-- Insert dummy data into Post table
INSERT INTO Post (PostedDate, Note, PlaceId, Status, IsBlock, StaffId) VALUES 
(GETDATE(), 'Post Note 1', 1, 1, 0, 1),
(GETDATE(), 'Post Note 2', 2, 1, 0, 2);

-- Insert dummy data into FavoritePost table
INSERT INTO FavoritePost (AccountId, PostId, CreatedAt) VALUES 
(1, 1, GETDATE()),
(2, 2, GETDATE());

-- Insert dummy data into Image table
INSERT INTO Image (ImageUrl, PostId) VALUES 
('https://example.com/post1_image1.jpg', 1),
('https://example.com/post2_image1.jpg', 2);

-- Insert dummy data into Interior table
INSERT INTO Interior (InteriorName, Status, Description, PlaceId) VALUES 
('Modern', 1, 'Modern interior design', 1),
('Classic', 1, 'Classic interior design', 2);
