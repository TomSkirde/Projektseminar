import './App.css';
import React from 'react';
import FileUploadButton from './components/FileUploadButton';
import GetJsonsButton from './components/GetJsonsButton';

const App: React.FC = () => {
    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <FileUploadButton />
            <GetJsonsButton />
        </div>

    );
};

export default App;