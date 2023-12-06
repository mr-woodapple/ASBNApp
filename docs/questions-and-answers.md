## How to process data?
I need to read content from a file and store it somewhere globally.

Current flow:
- Data is loaded through the "Open File" button in the navbar -> and content extracted + passend to an instance of LocalASBNDataService
- I want to access it from the MainViewDay (for example) and get the exact same data that I just read from the file (which is in another instance)

## Comments
- Singletons seem to be a bad idea for general code quality / testing
- Dependency Injection? But how would that solve my problem?


## Questions
- Why are we injecting IASBNDataService (the interface) - not an instantiation of it? -> what happens if I have multiple classes implementing that interface? Is the error that we're registering it as a Singleton?
