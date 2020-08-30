import React, { useReducer, useState } from "react";
import { Table, Button } from "semantic-ui-react";
import Rows from "./Rows";
import api from "../api/api";
//Add Folder --remove folder

const DataTable = ({ rows }) => {
  //here i would usereduer to filter rendered data in parent folder

  const [input, setInput] = useState("");

  function handleSearch() {
    let query = input;
    debugger;
    //TODO: Call Search api and pass query param ,await response and populate data

    //reset search state
    setInput("");
  }
  async function handleClick() {
    const testPath = "testFolder/innerFolder/providersReact.png";

    const data = await api.getFolderContents(testPath);
    debugger;
  }
  return (
    <div>
      <div style={{ display: "flex" }}>
        <div id="search" className="ui icon input">
          <input
            type="text"
            placeholder="Search..."
            value={input}
            onChange={(e) => setInput(e.target.value)}
          />
          <i className="circular search link icon" onClick={handleSearch} />
        </div>
        <Button circular={true}>Add Folder</Button>
        <Button circular={true}>Add File</Button>
        <Button circular={true} onClick={handleClick}>
          Get folder contents
        </Button>
      </div>
      <Table celled striped>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell colSpan="3">File Storage</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          <Rows rows={rows} />
        </Table.Body>
      </Table>
    </div>
  );
};

export default DataTable;
