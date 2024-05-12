# netscrape

**netscrape** is a CLI tool for Google searching.

It fetches results using specified search terms and prints simplified search results directly in the terminal.

*This tool **does not** require a Google API key to use.*

### Features

- **Google search**: Prints Google search results for specified search terms.
- **Link analysis**: Extracts and displays detailed information about each link.
- **Text wrapping**: Wraps text descriptions for better readability.

### Getting Started

1. Clone the repository.

   ```bash
   git clone https://github.com/fuseraft/netscrape
   cd netscrape
   ```

2. Build netscrape

   ```bash
   dotnet build
   ```

3. Execute the program with your desired search terms.

   ```bash
   netscrape "The Galileo Project"
   ```

### Requirements

- .NET 8.0
- HtmlAgilityPack