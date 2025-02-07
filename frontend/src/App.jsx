import { useState } from "react";
import CardList from "../Components/Card/CardList";
import Search from "../Components/Search/Search";
import "./App.css";

// function App() {
//   return (
//     <div className="app">
//       <Search />
//       <CardList />
//     </div>
//   );
// }

function App() {
  const [newItem, setNewItem] = useState("");
  const [toDoList, setToDoList] = useState([]);

  function addNewItem(e) {
    e.preventDefault();
    setToDoList((currList) => {
      return [
        ...currList,
        {
          id: crypto.randomUUID(),
          title: newItem,
          completed: false,
        },
      ];
    });
    setNewItem("");
  }

  function toggleToDo(id, completed) {
    setToDoList((currList) => {
      return currList.map((todo) => {
        if (todo.id === id) {
          return { ...todo, completed };
        }
        return todo;
      });
    });
  }

  function deleteToDo(id) {
    setToDoList((currList) => {
      return currList.filter((todo) => todo.id !== id);
    });
  }

  return (
    <>
      <form onSubmit={addNewItem} className="add-item-form">
        <div className="form-row">
          <label htmlFor="item">Add New Item</label>
          <input
            value={newItem}
            onChange={(e) => setNewItem(e.target.value)}
            type="text"
            id="item"
          />
        </div>
        <button className="btn">Add Item</button>
      </form>
      <h1 className="header">ToDo List</h1>
      <ul className="list">
        {toDoList.length === 0 && "No Items to do"}
        {toDoList.map((todo) => {
          return (
            <li key={todo.id}>
              <label>
                <input
                  type="checkbox"
                  checked={todo.completed}
                  onChange={(e) => toggleToDo(todo.id, e.target.checked)}
                />
                {todo.title}
              </label>
              <button
                onClick={() => deleteToDo(todo.id)}
                className="btn btn-danger"
              >
                Delete
              </button>
            </li>
          );
        })}
      </ul>
    </>
  );
}

export default App;
