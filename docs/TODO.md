# Todo List ASBN App
This is the markdown todo file for the ASBN App project.



## General

- [x] Navbar Design (define ui structure in MainLayout file)
- [x] Nav link component (active / inactive)
- [ ] Save data dialog (update with state from backend)
- [x] Date Picker (as a pop-up / overlay)
- [x] Component pill as a selector (used on DailyStart for example)
- [ ] Info message if the app can't be used on my device (not supported by browser / device)


## Start Page

Split into a weekly and a daily view.

- [x] Component for the main header ('let's see what you've been up to' for example)
- [x] Component Weekly / Daily view switcher


#### Weekly page:
  - [x] Week selector -> opens date picker on click
  - [x] Overall page ui
  - [x] Day component (incl. Day, Notes, Location)
  - [x] Show date picker on click on a date (not everyone might be working Mon - Fri)
  - [x] Check if data for the current week is present at loading -> load that
  - [ ] Track changes, show "Update" button if user changed existing data / show "Save" button for first input

#### Daily page:
  - [x] Page ui
  - [x] Check if data for the current day is present in the local .JSON -> load that
  - [x] show date picker when user clicks on "Pick different date", use that value going forward
  - [x] Get values for date pickers, display them
  - [ ] Get values for work location from JSON
  - [ ] Track changes, show "Update" button if user changed existing data / "Save" button for first input


## Export Page 

Let's the user create and sign the PDFs, save them locally.

- [x] Overall page ui/layout
- [ ] Display pill selector with current week (with next and prev one) & dates of that week
- [ ] Generate pdf from selected week
- [ ] Preview window (optional!)
- [ ] Read MyCard data (get name)
- [ ] Create signature image (close to the Telekom signature UI)




## Settings Page 

- [x] Overall UI
- [x] Load data from JSON & display it here
- [ ] Write user job from JSON -> select from a predefined list
- [ ] Write user locations/hours, add new entries
- [ ] Write user legal representitive
- [ ] Write user company & school


## Backend

- [x] C# Object Entry -> Date, Location?, Hours?, Notes?
- [x] C# Object User -> Name, Job, Locations/Working hours, LegalRepresentitive?, CompanyName?, SchoolName? (-> now two objects called Settings & WorkLocationHours)
- [ ] Create .json files on the users system
- [ ] Read/Write files on the users system using the FileSystemAccess API (helpful? https://github.com/KristofferStrube/Blazor.FileSystemAccess)
- [ ] Get username from the myCard, use that on the document where it says "name"
- [ ] Store user settings in JSON file
- [ ] Get save state, update status bar with current save state
- [ ] Sign PDF files  
- [ ] Use exisiting pdf template, index available input fields



