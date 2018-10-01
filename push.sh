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
  git remote add origin-deploy https://${GH_USERNAME}:${GH_PASSWORD}@github.com/Simon1612/BachelorProjekt.git
  git push https://${GH_USERNAME}:${GH_PASSWORD}@github.com/Simon1612/BachelorProjekt.git #--quiet --set-upstream origin deploy
}

setup_git
commit_website_files
upload_files
