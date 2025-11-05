# MAUI LayoutCycleException Repro – Border in ControlTemplates

[dotnet/maui#32406](https://github.com/dotnet/maui/issues/32406)

This repository contains a minimal .NET MAUI reproduction project that demonstrates a `LayoutCycleException` occurring on **Windows** when the UI contains a large number of deeply nested layouts that use **`Border`** elements inside control templates.

When the same UI is generated **without** Borders (or using Telerik `RadBorder`), the crash never occurs. This strongly suggests that the issue is triggered by MAUI `Border` layout logic.

---

## ✅ Problem Summary

A layout cycle occurs during the native WinUI layout pass when many nested controls contain MAUI `Border` elements. The crash is intermittent in smaller visual trees, but consistently reproducible when a large number of elements and layout depth are used.

**Exception thrown:**

```
Microsoft.UI.Xaml.dll!LayoutCycleDetected
Layout cycle detected. Layout was invalidated during measure or arrange.
```


- Only reproducible on **Windows**
- Does **not** repro on Android, iOS, or MacCatalyst
- Removing Borders or replacing them with RadBorder eliminates the issue

---

## ✅ How to Run the Repro

1. Clone the repository
2. Open the solution in Visual Studio 2022 (or later)
3. Set the project to run as **Windows** and start debugging
4. Use the UI to reproduce the crash (see below)

---

## ✅ Repro Steps Using This Sample App

This app exposes UI controls that deliberately generate a large layout tree.

### Controls on the Main Page

| Control | Purpose |
|---------|---------|
| **Add Stuff** | Adds nested controls *with Borders* — this will eventually crash |
| **Add No Border Stuff** | Adds the same controls, but with **no** Borders — this will not crash |
| **Element Count Slider** | Sets how many elements to add |
| **Depth Slider** | Sets how many nested levels each element contains |
| **Clean Up** | Removes all constructed elements |

### Steps to reproduce the crash

1. Launch the app on **Windows**
2. Increase the **Element Count** slider (10–30 usually triggers it)
3. Increase the **Depth** slider (3 or more)
4. Click **Add Stuff**
5. After layout cycles process → the application will throw:

```
LayoutCycleException: Layout was invalidated during measure or arrange.
```


### Steps to verify the workaround

1. Using the same slider values
2. Click **Add No Border Stuff**
3. App builds the same layout but **never crashes**

This demonstrates that Borders in control templates are the trigger.

---

## ✅ Expected Behavior
The layout should complete without entering an infinite measure/arrange loop, regardless of whether Borders are present.

## ❌ Actual Behavior
- App crashes with LayoutCycleException
- The debugger breaks into native WinUI layout code
- No managed stack frames are available
- Removing Borders or replacing with RadBorder prevents the crash

---

## ✅ Environment

- .NET: tested on .NET 7 and .NET 8
- Platform: Windows only
- UI toolkit: .NET MAUI / WinUI backend

---

## ✅ Related MAUI / WinUI Questions for Maintainers

- Is this a known issue with the MAUI `Border` control?
- Is there guidance on avoiding nested Border usage in templates?
- Is there a recommended diagnostic method to capture native layout traces for this scenario?
- If the issue is inside WinUI’s layout engine, is there a recommended workaround besides replacing every Border?

---

## ✅ Files Included

| File / Folder | Description |
|---------------|-------------|
| `/src` | Minimal MAUI app source demonstrating the issue |
| `README.md` | Reproduction instructions (this file) |
| (Optional) `dumps/` | Example dump files if included |

---

## ✅ Contact & Context

This repro originated from real production crashes in a large MAUI application. Removing Borders or using Telerik RadBorder fully resolves the issue, so this repo is intended to help the MAUI team identify and fix the root cause.

---

If you have any questions, need additional traces, or want a version that logs native layout cycles, we are happy to provide anything helpful.
