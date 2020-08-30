import React, { useState } from "react";
import "./App.css";
import DataTable from "./components/DataTable";
import api from "./api/api";
//mock data
const rowsData = [
  { fileName: "file1" },
  { folderName: "folder2" },
  { fileName: "file2" },
  { fileName: "folder3" },
  { folderName: "asdf" },
  { folderName: "asdf2" },
  { folderName: "asdf4" },
  { fileName: "file6" },
  { fileName: "file7" },
  { fileName: "file8" },
  { fileName: "file9" },
  { fileName: "file10" },
  { fileName: "file11" },
  { fileName: "file12" },
  { fileName: "file13" },
  { fileName: "file14" },
  { fileName: "file15" },
  { fileName: "file16" },
  { fileName: "file17" },
  { fileName: "file18" },
  { fileName: "file19" },
  { fileName: "file20" },
  { fileName: "file21" },
];
function App() {
  async function handleChange(event) {
    let files = event.target.files;

    //Test path
    const tempPath = "testFolder/innerFolder";
    const data = await api.uploadFiles(files, tempPath);
    debugger;
  }

  return (
    <div className="App">
      <h4>Upload files here</h4>
      <input type="file" id="fileshare" multiple onChange={(e) => handleChange(e)} />
      <DataTable rows={rowsData} />
    </div>
  );
}

export default App;
/**
 * DnD
 * <see>https://www.raymondcamden.com/2019/08/08/drag-and-drop-file-upload-in-vuejs
 *
 */
