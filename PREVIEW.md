# 🏋️‍♂️ FitnessTracker - Professional Fitness & Diet Management System

## 📱 Application Preview

### 🎨 **Professional Login System**
![Login Page](https://via.placeholder.com/800x400/667eea/ffffff?text=Professional+Login+Page)

**Features:**
- ✅ Modern Bootstrap 5 design
- ✅ Smooth animations and transitions
- ✅ Password strength indicators
- ✅ Real-time form validation
- ✅ Mobile-responsive layout

### 🔐 **OTP Verification System**
![OTP Verification](https://via.placeholder.com/800x400/0dcaf0/ffffff?text=OTP+Verification+Page)

**Features:**
- ✅ 6-digit OTP input with auto-focus
- ✅ Mobile number verification
- ✅ Resend OTP functionality
- ✅ Professional error handling
- ✅ Secure authentication flow

### 📊 **Interactive Dashboard**
![Dashboard](https://via.placeholder.com/800x400/198754/ffffff?text=Interactive+Dashboard)

**Features:**
- ✅ Statistics cards with animations
- ✅ Progress tracking visualization
- ✅ Recent activities tabs
- ✅ Quick action buttons
- ✅ Real-time data updates

### 💪 **Fitness Tracking**
![Fitness Tracking](https://via.placeholder.com/800x400/dc3545/ffffff?text=Fitness+Tracking+Interface)

**Features:**
- ✅ Workout logging and tracking
- ✅ Exercise library with instructions
- ✅ Calorie calculation
- ✅ Progress monitoring
- ✅ Customizable workout plans

### 🍎 **Diet Management**
![Diet Management](https://via.placeholder.com/800x400/ffc107/ffffff?text=Diet+Management+System)

**Features:**
- ✅ Meal planning and tracking
- ✅ Nutritional information
- ✅ Calorie counting
- ✅ Macro tracking (Protein, Carbs, Fat)
- ✅ Diet categories and customization

### 🏆 **Rewards System**
![Rewards System](https://via.placeholder.com/800x400/6f42c1/ffffff?text=Rewards+and+Achievements)

**Features:**
- ✅ Point-based reward system
- ✅ Daily streak tracking
- ✅ Achievement badges
- ✅ Progress visualization
- ✅ Motivation incentives

## 🚀 **Key Features Overview**

### **Authentication & Security**
- 🔐 Secure login with OTP verification
- 📱 Mobile number-based authentication
- 🔒 Password hashing with SHA256
- 🛡️ Session management
- ✅ Form validation with real-time feedback

### **Fitness Tracking**
- 💪 Comprehensive workout logging
- 📊 Progress statistics and analytics
- 🎯 Goal setting and monitoring
- 📈 Visual progress charts
- 🏃‍♂️ Exercise library with instructions

### **Diet Management**
- 🍎 Meal planning and tracking
- 📊 Nutritional analysis
- 🎯 Calorie and macro tracking
- 📱 Mobile-responsive interface
- 🍽️ Customizable diet plans

### **Rewards & Motivation**
- 🏆 Point-based reward system
- 🔥 Daily streak tracking
- 🎖️ Achievement badges
- 📈 Progress visualization
- 💪 Motivation incentives

### **Professional UI/UX**
- 🎨 Modern Bootstrap 5 design
- ✨ Smooth animations and transitions
- 📱 Mobile-first responsive design
- 🎯 Intuitive user interface
- 🌈 Professional color scheme

## 🛠️ **Technology Stack**

### **Frontend**
- **ASP.NET MVC** - Web framework
- **C#** - Programming language
- **Bootstrap 5** - CSS framework
- **JavaScript/jQuery** - Interactivity
- **Font Awesome** - Icons
- **Chart.js** - Data visualization

### **Backend**
- **PL/SQL** - Database procedures
- **Entity Framework Core** - ORM
- **Oracle Database** - Data storage
- **ASP.NET Core** - Web framework

### **Features**
- 🔄 Real-time data updates
- 📊 Interactive charts and graphs
- 🎨 Professional animations
- 📱 Mobile-responsive design
- 🛡️ Secure authentication
- 📈 Progress tracking

## 📋 **Database Schema**

### **Core Tables**
- **Users** - User authentication and profiles
- **UserOTP** - OTP verification system
- **Workouts** - Workout plans and exercises
- **WorkoutLogs** - User workout tracking
- **Diets** - Diet plans and nutrition
- **MealLogs** - User meal tracking
- **Rewards** - Achievement and reward system
- **DailyStreaks** - Daily discipline tracking

### **Stored Procedures**
- User authentication and registration
- OTP generation and validation
- Workout and meal logging
- Statistics calculation
- Reward management
- Data retrieval and reporting

## 🎯 **User Experience**

### **Login Flow**
1. **Username/Password** entry
2. **OTP Generation** and SMS sending
3. **OTP Verification** with 6-digit input
4. **Dashboard Access** with personalized data

### **Dashboard Experience**
1. **Welcome Section** with personalized greeting
2. **Statistics Cards** showing key metrics
3. **Recent Activities** with tabbed interface
4. **Quick Actions** for common tasks
5. **Progress Charts** for visual feedback

### **Tracking Experience**
1. **Workout Logging** with duration and calories
2. **Meal Tracking** with nutritional data
3. **Progress Monitoring** with statistics
4. **Reward Earning** through discipline
5. **Achievement Unlocking** for motivation

## 🔧 **Setup Instructions**

### **Prerequisites**
- .NET 6.0 SDK
- Oracle Database
- Visual Studio 2022 or VS Code

### **Installation Steps**
1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/FitnessTracker.git
   cd FitnessTracker
   ```

2. **Set up the database**
   ```sql
   -- Run the database scripts
   @Database/CreateTables.sql
   @Database/StoredProcedures.sql
   ```

3. **Configure connection string**
   ```json
   // appsettings.json
   "ConnectionStrings": {
     "DefaultConnection": "your-oracle-connection-string"
   }
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

## 📱 **Mobile Responsive Design**

The application is fully responsive and works seamlessly on:
- 📱 Mobile phones
- 📱 Tablets
- 💻 Desktop computers
- 🖥️ Large screens

## 🎨 **Design Features**

### **Color Scheme**
- **Primary**: Blue gradient (#667eea to #764ba2)
- **Success**: Green gradient (#11998e to #38ef7d)
- **Warning**: Orange gradient (#f093fb to #f5576c)
- **Info**: Cyan (#0dcaf0)

### **Animations**
- ✨ Smooth fade-in effects
- 🎯 Hover animations
- 📊 Progress bar animations
- 🔄 Loading spinners
- 📱 Touch-friendly interactions

### **Typography**
- **Font**: Poppins (Google Fonts)
- **Weights**: 300, 400, 500, 600, 700
- **Icons**: Font Awesome 6.4.0

## 🚀 **Ready to Deploy**

The application is production-ready with:
- ✅ Professional authentication system
- ✅ Comprehensive fitness tracking
- ✅ Advanced diet management
- ✅ Motivational rewards system
- ✅ Mobile-responsive design
- ✅ Secure database architecture
- ✅ Scalable code structure

---

**🎉 Start your fitness journey with FitnessTracker today!** 