import React from "react";
import { Table, Icon } from "semantic-ui-react";

const Rows = ({ rows }) => {
  return rows.map((row) => (
    <Table.Row>
      <Table.Cell collapsing>
        {row.folderName ? <Icon name="folder" /> : <Icon name="file outline" />}
      </Table.Cell>
      {row.folderName ? (
        <Table.Cell>{row.folderName}</Table.Cell>
      ) : (
        <Table.Cell>{row.fileName}</Table.Cell>
      )}
      {/* <Table.Cell collapsing textAlign="right">
        10 hours ago
      </Table.Cell> */}
    </Table.Row>
  ));
};

export default Rows;
