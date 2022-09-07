# Jellylyrics *Alpha - Don't build to it yet. Still subject to API changes.

Adds new API endpoint `/Items/%ITEMID%/Lyrics` that returns local LRC files as JSON.

## Install
1. In jellyfin, go to dashboard -> plugins -> Repositories -> add and paste this link https://raw.githubusercontent.com/1hitsong/jellylyrics/main/manifest.json

2. Go to Catalog and search Jellylyrics

3. Click on it and install

4. Restart Jellyfin

## How To Use
1. Have a folder with a file and a corresponding LRC file on your server

2. Call the API endpoint from your API client

3. Read and use returned JSON data

4. User is happy to see lyrics

## Notes
1. Only works with LRC files

2. LRC files must be in same folder as the requested item

3. LRC files must have exact same filename, excluding file extenstion, as its corresponding item

## Responses
200 
Lyrics found and JSON results returned

404 
Requested Item not found, LRC file not found, JSON Converstion failed, 
