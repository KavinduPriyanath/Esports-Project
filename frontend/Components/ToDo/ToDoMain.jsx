import { ToDoForm } from "../ToDo/ToDoForm";
import { ItemList } from "../ToDo/ListItem";
import { useEffect, useState } from "react";

export function ToDoMain() {
  const [toDoList, setToDoList] = useState(() => {
    const localValue = localStorage.getItem("ITEMS");
    if (localValue == null) return [];
    return JSON.parse(localValue);
  });

  useEffect(() => {
    localStorage.setItem("ITEMS", JSON.stringify(toDoList));
  }, [toDoList]);

  function addToDo(title) {
    setToDoList((currList) => {
      return [
        ...currList,
        {
          id: crypto.randomUUID(),
          title: title,
          completed: false,
        },
      ];
    });
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
      <ToDoForm onAddItem={addToDo} />
      <h1 className="header">ToDo List</h1>
      <ItemList
        toDoList={toDoList}
        toggleToDo={toggleToDo}
        deleteToDo={deleteToDo}
      />
    </>
  );
}
