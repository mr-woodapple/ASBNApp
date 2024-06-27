![ASBN app logo, orange on light grey background](https://github.com/mr-woodapple/ASBNApp/blob/main/assets/logo-asbnapp/Logo-Wide-Orange-wBG.png)
# ASBN App
A Blazor Web Assembly app that allows you to write your mandatory apprenticeship report book with ease. The aim is to be able to quickly add a note for a day before forgetting about it, and when the time has come, you can export a ready-to-go pdf with the information you entered sometime before.

To avoid any data security concerns, the app uses a local `JSON` file to store information. This needs to be loaded every time a user visits the website. The PDF file is then updated whenever the uses hit's "Save" in the app.

## Install
Clone the repository into a folder of your choice.

## Usage
Navigate into `/src/ASBApp/`, then run the following command:
```
dotnet watch run
```
> There's a demo `Sample.json` file in the `/docs/sampleJSON` folder, which can be used to explore the app.

## Contributing
PRs accepted. Looking forward to your ideas / bugfixes, whatever you may wanna do. :)

## License
MIT © mr-woodapple 2023

## 💕 Credits
Many thanks to the people behind these packages that made my life a lot easier! 

- [Blazor.FileSystemAccess](https://github.com/KristofferStrube/Blazor.FileSystemAccess) by Kristoffer Strube
- [PDFsharp](https://github.com/empira/PDFsharp) by the folks behind empira
- [MudBlazor](https://mudblazor.com/) 
