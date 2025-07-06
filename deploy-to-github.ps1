# 🚀 Fitness & Diet Tracker - GitHub Deployment Script

Write-Host "🚀 Fitness & Diet Tracker - GitHub Deployment" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Green

# Set Git alias
Set-Alias -Name git -Value "C:\Program Files\Git\bin\git.exe"

Write-Host "`n📋 Step 1: GitHub Repository Setup" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

Write-Host "1. Go to https://github.com and sign in" -ForegroundColor White
Write-Host "2. Click the '+' icon and select 'New repository'" -ForegroundColor White
Write-Host "3. Repository name: fitness-diet-tracker" -ForegroundColor White
Write-Host "4. Description: Professional Fitness and Diet Tracking Application" -ForegroundColor White
Write-Host "5. Choose Public or Private" -ForegroundColor White
Write-Host "6. DO NOT initialize with README (we already have one)" -ForegroundColor White
Write-Host "7. Click 'Create repository'" -ForegroundColor White

$githubUsername = Read-Host "`nEnter your GitHub username"
$repositoryName = "fitness-diet-tracker"

Write-Host "`n🔗 Step 2: Connecting to GitHub" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

# Remove existing remote if any
git remote remove origin 2>$null

# Add new remote
git remote add origin "https://github.com/$githubUsername/$repositoryName.git"

Write-Host "✅ Remote origin added: https://github.com/$githubUsername/$repositoryName.git" -ForegroundColor Green

Write-Host "`n📤 Step 3: Pushing to GitHub" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

git branch -M main
git push -u origin main

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Successfully deployed to GitHub!" -ForegroundColor Green
    Write-Host "🌐 Repository URL: https://github.com/$githubUsername/$repositoryName" -ForegroundColor Cyan
    
    Write-Host "`n📋 Step 4: Live Deployment Options" -ForegroundColor Yellow
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    Write-Host "Choose your deployment platform:" -ForegroundColor White
    Write-Host "1. Azure App Service (Recommended - Free tier available)" -ForegroundColor White
    Write-Host "2. Heroku (Free tier available)" -ForegroundColor White
    Write-Host "3. Railway (Free tier available)" -ForegroundColor White
    Write-Host "4. DigitalOcean App Platform" -ForegroundColor White
    
    $deploymentChoice = Read-Host "`nEnter your choice (1-4)"
    
    switch ($deploymentChoice) {
        "1" {
            Write-Host "`n🔵 Azure App Service Setup:" -ForegroundColor Blue
            Write-Host "1. Go to https://portal.azure.com" -ForegroundColor White
            Write-Host "2. Create a free account" -ForegroundColor White
            Write-Host "3. Create App Service" -ForegroundColor White
            Write-Host "4. Connect to GitHub repository" -ForegroundColor White
            Write-Host "5. Set up continuous deployment" -ForegroundColor White
            Write-Host "6. Configure database connection" -ForegroundColor White
        }
        "2" {
            Write-Host "`n🟣 Heroku Setup:" -ForegroundColor Magenta
            Write-Host "1. Go to https://heroku.com" -ForegroundColor White
            Write-Host "2. Create account" -ForegroundColor White
            Write-Host "3. Install Heroku CLI" -ForegroundColor White
            Write-Host "4. Run: heroku create fitness-diet-tracker-app" -ForegroundColor White
            Write-Host "5. Run: git push heroku main" -ForegroundColor White
        }
        "3" {
            Write-Host "`n🟡 Railway Setup:" -ForegroundColor Yellow
            Write-Host "1. Go to https://railway.app" -ForegroundColor White
            Write-Host "2. Sign in with GitHub" -ForegroundColor White
            Write-Host "3. Connect your repository" -ForegroundColor White
            Write-Host "4. Auto-deploy on push" -ForegroundColor White
        }
        "4" {
            Write-Host "`n🔴 DigitalOcean Setup:" -ForegroundColor Red
            Write-Host "1. Go to https://cloud.digitalocean.com" -ForegroundColor White
            Write-Host "2. Create account" -ForegroundColor White
            Write-Host "3. Create App Platform" -ForegroundColor White
            Write-Host "4. Connect GitHub repository" -ForegroundColor White
        }
        default {
            Write-Host "`n❌ Invalid choice. Please run the script again." -ForegroundColor Red
        }
    }
    
    Write-Host "`n📖 Detailed instructions available in: GITHUB_DEPLOYMENT_GUIDE.md" -ForegroundColor Cyan
    Write-Host "🔧 Database setup instructions in: Database/CreateTables.sql" -ForegroundColor Cyan
    
} else {
    Write-Host "`n❌ Failed to push to GitHub. Please check:" -ForegroundColor Red
    Write-Host "1. Repository URL is correct" -ForegroundColor White
    Write-Host "2. You have access to the repository" -ForegroundColor White
    Write-Host "3. GitHub credentials are configured" -ForegroundColor White
}

Write-Host "`n🎉 Deployment script completed!" -ForegroundColor Green
Write-Host "📚 For detailed instructions, see: GITHUB_DEPLOYMENT_GUIDE.md" -ForegroundColor Cyan 