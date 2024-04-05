# Blogging Web Application for Bislerium PVT. LTD.

## Project Overview

Bislerium PVT. LTD. requires a web application for blogging to integrate into their social media platform. The application will support three user categories: bloggers, admins, and surfers. The application is built on an enterprise-level framework and has several stringent specifications.

## Features

### For Bloggers

1. **Profile Management**
    - Register, view, update, and delete profiles.
    - Change password and reset via email.
   
2. **Blog Management**
    - Create, edit, and delete blog posts.
    - Upvote or downvote blog posts.
    - Comment on blog posts and react to comments with upvote or downvote.
    - Update or delete own comments.

3. **Notifications**
    - Receive push notifications for activities on their posts.

### For Surfers

1. **Browsing**
    - Browse paginated and sortable catalog of blogs.
    - Read blogs without login.

### For Admins

1. **User Management**
    - Create another admin.

2. **Dashboard**
    - Visualize all-time and monthly cumulative counts of blog posts, upvotes, downvotes, and comments.
    - Top 10 most popular blog posts and bloggers of all time and specific month.

## Metrics Calculation

- **Blog Popularity** = upvote weightage * upvotes + downvote weightage * downvotes + comment weightage * comments
    - Upvote Weightage: 2
    - Downvote Weightage: -1
    - Comment Weightage: 1

## Deliverables

### Software Artefact

- Visual Studio 2022 or above project.
- Source code and data files (excluding .git, .vs, .github, bin, and obj folders).

### Reflective Essay

1. **Instructions to Run the Program**
2. **Solution Design**
    - Concise description for each implemented function.
3. **Software Architecture**
    - Detailed breakdown of software classes, indicating original work and sourced classes.
4. **Class Properties and Methods**
    - Description of properties and methods for each class.
5. **Group Member Reflection**
    - Experience using Visual Studio, C# ASP.NET Core, and MS SQL.

## Marking Scheme

### Implementation of Application (80 Marks)

1. Profile Management: 10 Marks
2. Password Management: 4 Marks
3. Blog Management: 10 Marks
4. Image File Limit: 4 Marks
5. Blog Browsing: 6 Marks
6. Surfer Access: 4 Marks
7. Blog Reactions: 6 Marks
8. Blog Comments: 6 Marks
9. Comment Update/Delete: 4 Marks
10. Notifications: 10 Marks
11. History Preservation: 4 Marks
12. Admin Creation: 4 Marks
13. Admin Dashboard: 6 Marks
14. Dashboard Visualization: 6 Marks

## External Libraries

We encourage the use of third-party libraries to enhance the application's functionality.
