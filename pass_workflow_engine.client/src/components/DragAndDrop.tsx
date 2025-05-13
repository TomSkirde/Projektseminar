import React, { useState, useRef, DragEvent } from 'react';
import '../styles/DragAndDrop.css';
/*type FileUploadProps = {
    onFileSelected: (file: File) => void;
} */

const DragAndDrop: React.FC = () => {

    const [isDragging, setIsDragging] = useState<boolean>(false);
    const [file, setFile] = useState<File | null>(null);
    const [uploadStatus, setUploadStatus] = useState<string | null>(null);
    const fileInputRef = useRef<HTMLInputElement>(null);


    //Drag and drop events 
    const handleDrag = (e: DragEvent<HTMLDivElement>) => {
        e.preventDefault();
        switch (e.type) {
            case 'dragenter': setIsDragging(true); break;
            case 'dragleave': setIsDragging(false); break;
            case 'drop':
                if (e.dataTransfer.files?.length) {
                    setFile(e.dataTransfer.files[0]);
                }
                break;
        }
    }

    //Upload per click
    const handleClick = () => {
        fileInputRef.current?.click();
    }

    const handleFileInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) { 
            setFile(e.target.files[0]);
            console.log(file);
            handleFileUpload();
        }
    }

    const handleFileUpload = async () => {
        if (!file) return;

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
            setFile(null); // Clear the file after upload
        } catch (error) {
            console.error('Error uploading file:', error);
            setUploadStatus('An error occurred while uploading the file.');
        }
     }

    return (
        <div
            className='DragBox'
            onClick={handleClick}
            onDragEnter={handleDrag}
            onDragLeave={handleDrag}
            onDragOver={handleDrag}
            onDrop={handleDrag}
        >
            {isDragging ? 'Drop files here' : 'Drag & drop a file, or click to select'}
            <input
                type="file"
                ref={fileInputRef}
                onChange={handleFileInputChange}
                accept={'.owl'}
                style={{ display: 'none' }}
            />

        </div>

    )


}
export default DragAndDrop;

