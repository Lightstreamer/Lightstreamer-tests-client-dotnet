# Lightstreamer-tests-client-dotnet

Collects test projects for the Lightstreamer DotNet Client library

## Multi Repeated Connections

This project implemts a test case as described below:

1. Create three Lightstramer client object each for a different Lightstreaner client session.
2. repeat:
	3. Open the three client sessions (wait for a max 7 seconds).
	4. Subscribe some items for each session.
	5. Wait (max 5 seconds) for some data for each subscritpion.
	6. disconnect the three client sessions.
	7. Sleep 1 minute.
	