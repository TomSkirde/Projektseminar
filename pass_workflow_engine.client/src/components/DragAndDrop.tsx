import React, { useState, useCallback } from 'react';

const FileUploadButton: React.FC = () => {
    const [uploadStatus, setUploadStatus] = useState<string | null>(null);

    const handleFileUpload = async (file: File) => {
        const formData = new FormData();
        formData.append('owlfile', file);

        try {
            // Send the file to the backend API
            const response = await fetch('/api/Main/UploadFile', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                const result = await response.text();
                setUploadStatus(`File uploaded successfully: ${result}`);
            } else {
                setUploadStatus('Failed to upload file.');
            }
        } catch (error) {
            console.error('Error uploading file:', error);
            setUploadStatus('An error occurred while uploading the file.');
        }
    };

    const handleDrop = useCallback((event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        event.stopPropagation();
        const file = event.dataTransfer.files[0];
        if (file) handleFileUpload(file);
    }, []);

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) handleFileUpload(file);
    };

    const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
    };

    const handleButtonClick = () => {
        document.getElementById('fileInput')?.click();
    };

    return (
        <div>
            <div
                onDrop={handleDrop}
                onDragOver={handleDragOver}
                style={{
                    border: '2px dashed #ccc',
                    borderRadius: '10px',
                    padding: '20px',
                    textAlign: 'center',
                    marginBottom: '10px',
                    cursor: 'pointer',
                }}
            >
                Drag & Drop file here or <span onClick={handleButtonClick} style={{ color: 'blue', textDecoration: 'underline' }}>browse</span>
            </div>

            <input
                id="fileInput"
                type="file"
                accept=".owl"
                style={{ display: 'none' }}
                onChange={handleInputChange}
            />

            <p>{uploadStatus}</p>
        </div>
    );
};

export default FileUploadButton;