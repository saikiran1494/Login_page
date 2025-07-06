# GitHub Repository Setup Script
# Run this script after creating your GitHub repository

Write-Host "🚀 Setting up GitHub Repository for Fitness & Diet Tracker" -ForegroundColor Green
Write-Host "========================================================" -ForegroundColor Green

# Check if Git is available
try {
    $gitVersion = & "C:\Program Files\Git\bin\git.exe" --version
    Write-Host "✅ Git is available: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Git is not available. Please install Git first." -ForegroundColor Red
    exit 1
}

# Set Git alias
Set-Alias -Name git -Value "C:\Program Files\Git\bin\git.exe"

# Check current status
Write-Host "`n📋 Current Git Status:" -ForegroundColor Yellow
git status

# Prompt for GitHub username
$githubUsername = Read-Host "`nEnter your GitHub username"
$repositoryName = "fitness-diet-tracker"

Write-Host "`n🔗 Setting up remote origin..." -ForegroundColor Yellow
git remote add origin "https://github.com/$githubUsername/$repositoryName.git"

Write-Host "`n📤 Pushing to GitHub..." -ForegroundColor Yellow
git branch -M main
git push -u origin main

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Successfully pushed to GitHub!" -ForegroundColor Green
    Write-Host "🌐 Your repository is available at: https://github.com/$githubUsername/$repositoryName" -ForegroundColor Cyan
    
    Write-Host "`n📋 Next Steps:" -ForegroundColor Yellow
    Write-Host "1. Set up live deployment (see GITHUB_DEPLOYMENT_GUIDE.md)" -ForegroundColor White
    Write-Host "2. Configure database connection for production" -ForegroundColor White
    Write-Host "3. Set up CI/CD pipeline" -ForegroundColor White
    Write-Host "4. Configure domain and SSL" -ForegroundColor White
} else {
    Write-Host "`n❌ Failed to push to GitHub. Please check your repository URL." -ForegroundColor Red
}

Write-Host "`n📖 For detailed deployment instructions, see: GITHUB_DEPLOYMENT_GUIDE.md" -ForegroundColor Cyan 