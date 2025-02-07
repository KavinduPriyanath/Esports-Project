import { Item } from "./Item";

export function ItemList({ toDoList, toggleToDo, deleteToDo }) {
  return (
    <ul className="list">
      {toDoList.length === 0 && "No Items to do"}
      {toDoList.map((todo) => {
        return (
          <Item
            {...todo}
            key={todo.id}
            toggleToDo={toggleToDo}
            deleteToDo={deleteToDo}
          />
        );
      })}
    </ul>
  );
}
