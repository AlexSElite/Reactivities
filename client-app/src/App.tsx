import React, { Component } from "react";
import "./App.css";
import axios from "axios";
import { Header, Icon, List } from "semantic-ui-react";

class App extends Component {
  state = {
    values: [],
  };

  componentDidMount() {
    axios.get("http://localhost:5000/api/values").then((response) => {
      this.setState({
        values: response.data,
      });
    });
    //Fetch values from the API
    /*fetch("http://localhost:5000/api/values")
      .then((response) => response.json())
      .then((data) => {
        this.setState({
          values: data,
        });
      });*/
  }

  render() {
    const values = this.state.values;
    return (
      <div>
        <Header as="h2">
          <Icon name="users" />
          <Header.Content>Reactivities</Header.Content>
        </Header>

        <List>
          {values.map((value: any) => (
            <List.Item key={value.id}>{value.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;
