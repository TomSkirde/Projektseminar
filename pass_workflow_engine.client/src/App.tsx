import './App.css';
import React, { useState } from 'react';
import FileUploadButton from './components/FileUploadButton';
import GetJsonsButton from './components/GetJsonsButton';
import DragAndDrop from './components/DragAndDrop';
import ParseModelButton from './components/ParseModelButton';

const App: React.FC = () => {

    const [file, setFile] = useState<File | null>(null);

    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <GetJsonsButton />
            <DragAndDrop />
            <ParseModelButton />
        </div>

    );
};

export default App;