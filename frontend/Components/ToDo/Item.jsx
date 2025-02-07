export function Item({ id, title, completed, toggleToDo, deleteToDo }) {
  return (
    <li>
      <label>
        <input
          type="checkbox"
          checked={completed}
          onChange={(e) => toggleToDo(id, e.target.checked)}
        />
        {title}
      </label>
      <button onClick={() => deleteToDo(id)} className="btn btn-danger">
        Delete
      </button>
    </li>
  );
}
