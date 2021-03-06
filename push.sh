setup_git() {
  git config --global user.email "sp.16@hotmail.com"
  git config --global user.name ${GH_USERNAME}
}

commit_website_files() {
  git checkout -b deploy
  git add .
  git commit --message "Travis build: $TRAVIS_BUILD_NUMBER"
}

upload_files() {
  git remote add origin-deploy https://${GH_USERNAME}:${GH_PASSWORD}@github.com/Simon1612/BachelorProjekt.git #> /dev/null 2>&1
  git push https://${GH_USERNAME}:${GH_PASSWORD}@github.com/Simon1612/BachelorProjekt.git --force --quiet # > /dev/null 2>&1
}

setup_git
commit_website_files
upload_files
