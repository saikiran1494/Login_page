# 🚀 GitHub Repository Setup & Live Deployment Guide

## 📋 Prerequisites
- GitHub account
- Git installed (✅ Already done)
- .NET 9.0 SDK (✅ Already done)

## 🔧 Step 1: Create GitHub Repository

### 1.1 Go to GitHub.com
- Visit [github.com](https://github.com)
- Sign in to your account

### 1.2 Create New Repository
- Click the **"+"** icon in the top right
- Select **"New repository"**
- Fill in the details:
  - **Repository name**: `fitness-diet-tracker`
  - **Description**: `Professional Fitness & Diet Tracking Application with OTP verification, rewards system, and Oracle database integration`
  - **Visibility**: Choose Public or Private
  - **DO NOT** initialize with README (we already have one)
  - Click **"Create repository"**

## 🔗 Step 2: Connect Local Repository to GitHub

### 2.1 Add Remote Origin
```bash
git remote add origin https://github.com/YOUR_USERNAME/fitness-diet-tracker.git
```

### 2.2 Push to GitHub
```bash
git branch -M main
git push -u origin main
```

## 🌐 Step 3: Live Deployment Options

### Option A: Azure App Service (Recommended)
1. **Create Azure Account** (Free tier available)
2. **Deploy from GitHub**:
   - Go to Azure Portal
   - Create App Service
   - Connect to GitHub repository
   - Set up continuous deployment

### Option B: Heroku
1. **Create Heroku Account**
2. **Add Heroku Remote**:
   ```bash
   heroku create fitness-diet-tracker-app
   git remote add heroku https://git.heroku.com/fitness-diet-tracker-app.git
   ```

### Option C: Railway
1. **Create Railway Account**
2. **Connect GitHub Repository**
3. **Auto-deploy on push**

### Option D: GitHub Pages (Static Only)
- For static frontend only
- Not suitable for full ASP.NET Core app

## 🗄️ Step 4: Database Setup for Live Deployment

### 4.1 Oracle Cloud (Recommended)
1. **Create Oracle Cloud Account**
2. **Provision Oracle Database**
3. **Update Connection String**:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=your-oracle-host)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=your-service-name)));User Id=your-username;Password=your-password;"
     }
   }
   ```

### 4.2 Alternative: Azure SQL Database
1. **Create Azure SQL Database**
2. **Update Connection String**
3. **Modify Entity Framework for SQL Server**

## 🔐 Step 5: Environment Variables Setup

### 5.1 Create appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_PRODUCTION_CONNECTION_STRING"
  },
  "OTPSettings": {
    "ExpiryMinutes": 5,
    "Length": 6
  },
  "RewardsSettings": {
    "DailyPoints": 10,
    "StreakBonus": 5,
    "WeeklyBonus": 50
  }
}
```

### 5.2 Set Environment Variables
- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=https://+:443;http://+:80`

## 🚀 Step 6: Deployment Commands

### For Azure App Service:
```bash
# Build the application
dotnet publish -c Release -o ./publish

# Deploy to Azure (if using Azure CLI)
az webapp deployment source config-zip --resource-group YourResourceGroup --name YourAppName --src ./publish.zip
```

### For Heroku:
```bash
# Create Procfile
echo "web: dotnet FitnessTracker.dll" > Procfile

# Deploy
git push heroku main
```

## 📊 Step 7: Monitoring & Analytics

### 7.1 Application Insights (Azure)
- Add Application Insights to your app
- Monitor performance and errors

### 7.2 Health Checks
- Add health check endpoints
- Monitor database connectivity

## 🔧 Step 8: CI/CD Pipeline

### 8.1 GitHub Actions (Already Configured)
The `.github/workflows/dotnet.yml` file is already set up for:
- Build on push
- Run tests
- Deploy to staging

### 8.2 Customize for Your Deployment
Update the workflow file to deploy to your chosen platform.

## 📱 Step 9: Domain & SSL

### 9.1 Custom Domain
- Purchase domain (GoDaddy, Namecheap, etc.)
- Configure DNS settings
- Set up SSL certificate

### 9.2 SSL Certificate
- Most platforms provide free SSL
- Configure HTTPS redirects

## 🎯 Step 10: Testing Live Deployment

### 10.1 Test All Features
- [ ] User Registration
- [ ] OTP Verification
- [ ] User Login
- [ ] Dashboard Access
- [ ] Fitness Tracking
- [ ] Diet Tracking
- [ ] Rewards System
- [ ] Database Operations

### 10.2 Performance Testing
- Load testing
- Database performance
- Response times

## 📈 Step 11: Analytics & Monitoring

### 11.1 User Analytics
- Google Analytics
- User behavior tracking
- Conversion metrics

### 11.2 Application Monitoring
- Error tracking
- Performance monitoring
- Database monitoring

## 🔄 Step 12: Maintenance

### 12.1 Regular Updates
- Security patches
- Dependency updates
- Feature enhancements

### 12.2 Backup Strategy
- Database backups
- Code backups
- Configuration backups

## 🎉 Success Checklist

- [ ] GitHub repository created
- [ ] Code pushed to GitHub
- [ ] Live deployment configured
- [ ] Database connected
- [ ] Environment variables set
- [ ] SSL certificate configured
- [ ] Domain configured
- [ ] All features tested
- [ ] Monitoring set up
- [ ] Backup strategy implemented

## 🆘 Troubleshooting

### Common Issues:
1. **Database Connection**: Check connection string and firewall rules
2. **Build Errors**: Ensure all dependencies are included
3. **Runtime Errors**: Check environment variables
4. **Performance Issues**: Optimize database queries and caching

### Support Resources:
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Documentation](https://docs.microsoft.com/en-us/ef/)
- [Oracle Database Documentation](https://docs.oracle.com/en/database/)

---

## 🚀 Quick Start Commands

```bash
# 1. Create GitHub repository (manual)
# 2. Add remote and push
git remote add origin https://github.com/YOUR_USERNAME/fitness-diet-tracker.git
git push -u origin main

# 3. Deploy to your chosen platform
# 4. Update connection strings
# 5. Test all features
```

**Your application is ready for live deployment! 🎉** 