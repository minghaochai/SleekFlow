import { SnackbarProvider } from 'notistack';

import { ToDoList } from './features/todo/ToDoList';

function App() {
  return (
    <SnackbarProvider>
      <ToDoList />
    </SnackbarProvider>
  );
}

export default App;
