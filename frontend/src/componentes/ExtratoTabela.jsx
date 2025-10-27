import React from 'react'

export default function ExtratoTabela({ itens }){
  return (
    <table style={{width:'100%', marginTop:12, borderCollapse:'collapse'}}>
      <thead>
        <tr>
          <th style={{borderBottom:'1px solid #ddd', textAlign:'left'}}>Data</th>
          <th style={{borderBottom:'1px solid #ddd', textAlign:'left'}}>Valor</th>
        </tr>
      </thead>
      <tbody>
        {itens?.map(l => (
          <tr key={l.idTransacao}>
            <td style={{padding:'6px 0'}}>{new Date(l.dataTransacao).toLocaleString()}</td>
            <td style={{padding:'6px 0'}}>
              {l.valor < 0 ? '-' : ''}R$ {Math.abs(l.valor).toFixed(2)}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
