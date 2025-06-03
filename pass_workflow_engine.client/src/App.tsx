import React from 'react';
import { WorkflowDiagram } from './components/WorkflowDiagram';
import { GeneralButton } from './components/GeneralButton';
import { WorkflowProvider } from './context/WorkflowContext';
import './App.css';

function App() {
  const styles = {
    main: {
      minHeight: '100vh',
      backgroundColor: 'white',
      padding: '2rem',
      display: 'flex',
      flexDirection: 'column' as const,
      alignItems: 'center',
      gap: '4rem'
    },
    title: {
      fontSize: '2rem',
      fontWeight: '600',
      color: 'black',
      margin: 0
    },
    content: {
      display: 'flex',
      flexDirection: 'column' as const,
      alignItems: 'center',
      gap: '4rem',
      width: '100%',
      maxWidth: '1440px'
    }
  };

  return (
    <WorkflowProvider>
      <main style={styles.main}>
        <div style={styles.content}>
          <h1 style={styles.title}>PASS Workflow Engine</h1>
          <WorkflowDiagram />
          <div className="button-group">
            <GeneralButton type="upload" text="Modell einfügen" width={360} />
            <GeneralButton type="select" text="Modell auswählen" width={360} />
            <GeneralButton type="parse" text="Modell einlesen" width={360} />
          </div>
        </div>
      </main>
    </WorkflowProvider>
  );
}

export default App;