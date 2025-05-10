import './App.css';
import React from 'react';
import FileUploadButton from './components/FileUploadButton';

const App: React.FC = () => {
    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <FileUploadButton />
        </div>
    );
};

export default App;