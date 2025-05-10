import React, { useState } from 'react';

const FileUploadButton: React.FC = () => {
    const [uploadStatus, setUploadStatus] = useState<string | null>(null);

    const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            const file = event.target.files[0];

            // Create a FormData object to send the file
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
        }
    };

    const handleButtonClick = () => {
        // Programmatically trigger the file input dialog
        document.getElementById('fileInput')?.click();
    };

    return (
        <div>
            <button onClick={handleButtonClick}>Upload File</button>
            <input
                id="fileInput"
                type="file"
                accept=".owl"
                style={{ display: 'none' }}
                onChange={handleFileUpload}
            />
            {uploadStatus && <p>{uploadStatus}</p>}
        </div>
    );
};

export default FileUploadButton;
