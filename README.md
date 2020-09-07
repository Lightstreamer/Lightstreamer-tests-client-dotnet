# Lightstreamer-tests-client-dotnet

Collects test projects for the Lightstreamer DotNet Client library

## Multi Repeated Connections

This project implemts a test case as described below.

The app creates three LightstreamerClient objects each for a different Lightstreaner client session.
Then in a forever loop:
1. Open the three client sessions (wait for a max 7 seconds).
2. Subscribe some items for each session.
3. Wait (max 5 seconds) for some data for each subscritpion.
4. disconnect the three client sessions.
5. Sleep 1 minute.
	