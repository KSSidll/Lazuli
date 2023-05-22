# Project Lazuli

## Introduction

Social Media website that pulls data from JSONPlaceholder fake API

### Main features

-   Create and login with fake accounts that are bound to
    selected JSONPlaceholder profile
-   Waste your time on your profile feed filled with fake data,
    just like a real social media website!
-   Browse your and other profiles, check posts, comments and photo albums
-   Publish your own posts, Delete them, or Edit them \
    \* Data is only stored in Session, restarting the webpage will erase it
-   Find posts and users using a search bar

## How to run

Requires [.NET](https://dotnet.microsoft.com/en-us/download) sdk,
version 6.0 or higher

### Visual Studio / Raider

Open

    Lazuli.sln

Run project

### CLI

for development

    dotnet watch

for release

    dotnet publish "Lazuli/Lazuli.csproj" -c Release -o ./publish

or alternatively look at [Dockerfile](Dockerfile)

## Acceptance Tests and Project Details for Project Lazuli

### Platform/language/framework

    1. ASP.NET Razor
    2. C#

### Team members

    1. Filip Kociok
    2. Szymon Kolasa

### Acceptance tests

    1. The site opens on chromium
    2. User can log in
    3. Posts are displayed along with comments
    4. User can view other users' profiles
    5. The user's profile page displays their:
        1. Photo albums
        2. Posts with comments

### MoSCoW

    - Must have
        1. Login
        2. Registration
        3. Displaying posts, photos and comments
        4. Browsing user profiles
    - Should have
        1. Publishing posts
        3. Deleting posts, comments
        4. Editing posts
        5. Filtering posts
        6. Searching posts, users and comments
    - Could have
        1. Deleting photos
        2. Editing account data
    - Won't have
        1. Publishing photos
        2. Use without logging in
        3. Deleting accounts
        4. Publishing comments
        5. Editing comments
