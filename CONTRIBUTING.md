# Contributing Guidelines

[[_TOC_]]

# Basic guidelines

All changes you commit or submit by merge request should follow these simple guidelines:
* Use merge requests.
* Should build without new warnings or errors. Please use the project's solution file to drive the build and test process.
* Please do not squash your Merge Request commits into one commit. Split PR into many meaningful commits, we can review separately.
* Code should be free from profanities in any language.

# Commits in your merge requests should

* One commit should represent one meaningful change. E.g. Please do not add a new header file and in the same commit update project solution.
* Have short (72 chars or less) meaningful subjects.
* Separate subject from body with a blank line.
* Use the imperative mood in the subject line.
* Wrap lines at 72 characters if possible (E.g.: URLs are very hard to wrap).
* Use the body to explain what and why you have done something. In most cases, you can leave out details about how a change has been made ( Good commit message examples can be found [here](https://wiki.openstack.org/wiki/GitCommitMessages#Information_in_commit_messages).)
* Your merge request title should contain WindowsPerf JIRA ticket, which is prefixed WPERF-. Note: our GitLab JIRA integration requires developers to add it.
  * You can post your MR without JIRA ticket but we will require ticket number to merge.
  * WindowsPerf JIRA tickets are in format: WPERF-[0-9]+.
# Advice on merge requests

* Applying the single responsibility principle to merge requests is always a good idea. Try not to include some additional stuff into the merge request. For example, do not fix any typos other than your current context or do not add a tiny bug fix to a feature.
* Title and description is the first place where you can inform other developers about the changes
* Description of a merge request should always be prepared with the same attention, whether the merge request has a small or huge change.
* Always think that anybody could read your merge request anytime.
* You should build your code and test (if possible) before creating the merge request.
* Both reviewers and the author should be polite in the comments.

# If you have commit access

* Do NOT use `git push --force`.
* Use Merge Requests to suggest changes to other maintainers.

# Testing your changes

TODO

# Reporting Bugs

A good bug report, which is complete and self-contained, enables us to fix the bug. Before  you report a bug, please check the list of [issues](https://gitlab.com/Linaro/WindowsPerf/vs-extension/-/issues) and, if possible, try a bleeding edge code (latest source tree commit).


# Common LICENSE tags

(Complete list can be found at: https://spdx.org/licenses)

| Identifier   | Full name |
| ------------ | --------------------------------------- |
| BSD-3-Clause | BSD 3-Clause "New" or "Revised" License |