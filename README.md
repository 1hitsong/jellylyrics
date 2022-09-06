# Jellylyrics

Adds new API endpoint /Items/%ITEMID%/Lyrics that returns local lyric files as JSON.

## Notes
1. Only works with LRC files

2. LRC file must have exact same filename, excluding lrc extenstion, as its corresponding song.

## Responses
200 Lyrics found and JSON results returned

404 Requested Item not found, LRC file not found, JSON Converstion failed, 
