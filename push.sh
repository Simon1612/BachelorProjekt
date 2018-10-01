setup_git() {
  git config --global user.email "travis@travis-ci.org"
  git config --global user.name "Travis CI"
}

commit_website_files() {
  git checkout -b DeployBranch
  git add .
  git commit --message "Travis build: $TRAVIS_BUILD_NUMBER"
}

upload_files() {
  echo ${GH_USERNAME}
  git remote add origin-deploy https://${GH_USERNAME}:${GH_PASSWORD}@github.com/Simon1612/BachelorProjekt.git
  git push --quiet --set-upstream origin DeployBranch
}

setup_git
commit_website_files
upload_files
