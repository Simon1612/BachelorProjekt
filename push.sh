setup_git() {
  git config --global user.email "sp.16@hotmail.com"
  git config --global user.name ${GH_USERNAME}
}

commit_website_files() {
  git checkout -b DeployBranch
  git add .
  git commit --message "Travis build: $TRAVIS_BUILD_NUMBER"
}

upload_files() {
  git remote add origin-deploy https://${GH_TOKEN}@github.com/Simon1612/BachelorProjekt.git
  git push --quiet --set-upstream origin DeployBranch
}

setup_git
commit_website_files
upload_files
