# Rhino Anywhere
---

## About the Project

**Rhino Anywhere** offers a clean API interface to stream Rhino Pixels from a remote Rhino instance, allowing users to view -- and edit -- models on any web-connected device, regardless of local resource availability. The platform consists of:

- A Rhino plugin that streams a model viewport as video over WebRTC, and listens for incoming commands and manipulation events
- A JavaScript library for establishing a real-time connection to a remote Rhino instance and executing arbitrary commands against the active model

## How to Build & Pre-requisites
[Please see building source guide](BUILDSOURCE.md)

## Running
1. Clone this Repo & Build (OR install Yak)
2. Clone the ([sdk-js repo](https://github.com/rhino-anywhere/sdk-js))
3. Clone the ([sample-frontend repo](https://github.com/rhino-anywhere/sample-frontend))
4. Open Rhino
5. Run `StartRhinoAnywhere`
6. Run the server by following the guides in the respecive repos

## Contributing

See the open issues for a places to help out and get involved.
See the [open issues](https://github.com/rhino-anywhere/rhino-anywhere-plugin/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc) 

## Licensing

Distributed under the MIT license. See `License.md`for more information.

## Acknowledgements

A huge thanks to AEC Tech 2023 for arranging and hosting this event.
Please check out other hackathon projects and future hackathon events hosted by [AECTech](https://www.aectech.us/).
