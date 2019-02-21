import React, { Component } from 'react';
import './App.css';

const Todo = (props) => {
  return (
    <div>
      <div style={{ fontSize: '1.25em', fontWeight: 'bold' }}>{props.name}</div>
      <div><b>Description:</b> {props.description}</div>
      <div><b>Priority:</b> {props.priority}</div>
      <div><b>Responsible:</b> {props.responsible}</div>
      <div><b>Deadline:</b> {props.deadline}</div>
      <div><b>Status:</b> {props.status}</div>
      <div><b>Category:</b> {props.category}</div>
      {/* <div><b>Parent Id:</b> {props.parentId}</div> */}
      <br />
    </div>
  );
}

const TodoList = (props) => {
  return (
    <div>
      {props.todos.map(todo => <Todo key={todo.id} {...todo} />)}
    </div>
  );
}

class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      data: [],
    };
  }

  componentDidMount() {
    fetch('http://localhost:50100/api/todos/')
      .then(response => response.json())
      .then(data => this.setState({ data: data }));
  }

  render() {
    return (
      <div>
        <h1>My ToDo List</h1>
        <TodoList todos={this.state.data} />
      </div>
    )
  }
}
export default App