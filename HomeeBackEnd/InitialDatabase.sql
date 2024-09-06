USE [HomeeDB]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[ImageUrl] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[CitizenId] [nvarchar](50) NOT NULL,
	[Role] [int] NOT NULL,
	[BirthDay] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[GoogleUserId] [nvarchar](255) NULL,
	[IsGoogleAuthenticated] [bit] NOT NULL,
	[IsBlock] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryPlace]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryPlace](
	[CategoryPlaceId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[PlaceId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryPlaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[ContractId] [int] IDENTITY(1,1) NOT NULL,
	[RenderId] [int] NOT NULL,
	[PlaceId] [int] NOT NULL,
	[Duration] [bigint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conversation]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conversation](
	[ConversationId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastMessageId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ConversationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConversationParticipant]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConversationParticipant](
	[ConversationParticipantId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ConversationId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ConversationParticipantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FavoritePost]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavoritePost](
	[AccountId] [int] NOT NULL,
	[PostId] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC,
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ImageId] [int] IDENTITY(1,1) NOT NULL,
	[ImageUrl] [nvarchar](255) NOT NULL,
	[PostId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Interior]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interior](
	[InteriorId] [int] IDENTITY(1,1) NOT NULL,
	[InteriorName] [nvarchar](255) NOT NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[PlaceId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InteriorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[ConversationId] [int] NOT NULL,
	[SenderId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[UrlPath] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[ExpiredAt] [datetime] NOT NULL,
	[SubscribedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Place]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Place](
	[PlaceId] [int] IDENTITY(1,1) NOT NULL,
	[Province] [nvarchar](255) NOT NULL,
	[Distinct] [nvarchar](255) NOT NULL,
	[Ward] [nvarchar](255) NOT NULL,
	[Street] [nvarchar](255) NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[Area] [float] NOT NULL,
	[Direction] [int] NOT NULL,
	[NumberOfToilet] [int] NOT NULL,
	[NumberOfBedroom] [int] NOT NULL,
	[Rent] [float] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[IsBlock] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PlaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[PostedDate] [datetime] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[PlaceId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[IsBlock] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscription]    Script Date: 9/6/2024 8:45:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscription](
	[SubscriptionId] [int] IDENTITY(1,1) NOT NULL,
	[Price] [float] NOT NULL,
	[Duration] [bigint] NOT NULL,
	[SubscriptionName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[SubscriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ((0)) FOR [IsGoogleAuthenticated]
GO
ALTER TABLE [dbo].[CategoryPlace]  WITH CHECK ADD  CONSTRAINT [FK_CategoryPlace_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[CategoryPlace] CHECK CONSTRAINT [FK_CategoryPlace_Category]
GO
ALTER TABLE [dbo].[CategoryPlace]  WITH CHECK ADD  CONSTRAINT [FK_CategoryPlace_Place] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[Place] ([PlaceId])
GO
ALTER TABLE [dbo].[CategoryPlace] CHECK CONSTRAINT [FK_CategoryPlace_Place]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_Place] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[Place] ([PlaceId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_Place]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_Render] FOREIGN KEY([RenderId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_Render]
GO
ALTER TABLE [dbo].[ConversationParticipant]  WITH CHECK ADD  CONSTRAINT [FK_ConversationParticipant_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[ConversationParticipant] CHECK CONSTRAINT [FK_ConversationParticipant_Account]
GO
ALTER TABLE [dbo].[ConversationParticipant]  WITH CHECK ADD  CONSTRAINT [FK_ConversationParticipant_Conversation] FOREIGN KEY([ConversationId])
REFERENCES [dbo].[Conversation] ([ConversationId])
GO
ALTER TABLE [dbo].[ConversationParticipant] CHECK CONSTRAINT [FK_ConversationParticipant_Conversation]
GO
ALTER TABLE [dbo].[FavoritePost]  WITH CHECK ADD  CONSTRAINT [FK_FavoritePost_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[FavoritePost] CHECK CONSTRAINT [FK_FavoritePost_Account]
GO
ALTER TABLE [dbo].[FavoritePost]  WITH CHECK ADD  CONSTRAINT [FK_FavoritePost_Post] FOREIGN KEY([PostId])
REFERENCES [dbo].[Post] ([PostId])
GO
ALTER TABLE [dbo].[FavoritePost] CHECK CONSTRAINT [FK_FavoritePost_Post]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_Post] FOREIGN KEY([PostId])
REFERENCES [dbo].[Post] ([PostId])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_Post]
GO
ALTER TABLE [dbo].[Interior]  WITH CHECK ADD  CONSTRAINT [FK_Interior_Place] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[Place] ([PlaceId])
GO
ALTER TABLE [dbo].[Interior] CHECK CONSTRAINT [FK_Interior_Place]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Conversation] FOREIGN KEY([ConversationId])
REFERENCES [dbo].[Conversation] ([ConversationId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Conversation]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Sender] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Sender]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Account]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Owner] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Owner]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Subscription] FOREIGN KEY([SubscriptionId])
REFERENCES [dbo].[Subscription] ([SubscriptionId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Subscription]
GO
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_Owner] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Place] CHECK CONSTRAINT [FK_Place_Owner]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Place] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[Place] ([PlaceId])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_Place]
GO
