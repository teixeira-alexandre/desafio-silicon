import React, { useState } from 'react'
import TelaConta from './componentes/TelaConta'

export default function App(){
  const [idConta, setIdConta] = useState(1)
  return (
    <div style={{fontFamily:'sans-serif', maxWidth:900, margin:'20px auto'}}>
      <h2>Gestão de Contas - Sillicon Village</h2>
      <label>Id da conta:&nbsp;</label>
      <input value={idConta} onChange={e=>setIdConta(e.target.value)} type='number' min='1' />
      <TelaConta idConta={idConta} />
      <p style={{marginTop:30, fontSize:12, opacity:.7}}>Usando `VITE_API` para apontar para a API (http://localhost:5000)</p>
    </div>
  )
}
