## Local Setup Guide

### Running with locally hosted API

1. Set `SearchRankingApi` as startup project, run without debugging
2. Copy the base url
3. Under `SearchRankingApp` project, inside `App.config` change `ApiBaseUrl` to point to the locally running API
4. Set `SearchRankingApp` as startup project and run 

### Running with serverless
1. Under `SearchRankingApp` inside `App.config` just toggle the `ApiBaseUrl` to point to serverless instance (Azure functions)
2. Set `SearchRankingApp` as startup project and run
