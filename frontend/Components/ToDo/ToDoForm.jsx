import { useState } from "react";

export function ToDoForm(props) {
  const [newItem, setNewItem] = useState("");

  function addNewItem(e) {
    e.preventDefault();
    if (newItem === "") return;
    props.onAddItem(newItem);
    setNewItem("");
  }
  return (
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
  );
}
