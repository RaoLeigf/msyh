<template>
    <div class="attechment">
       <vue-preview :slides="list"></vue-preview>
    </div>
</template>

<script>

export default {
    name:"attechment",
    props:{
      attachmentList:{
          type:Array,
          default:[]
      }
    },
    data(){
        return {
            fileList:[],
            list:[]
        }
    },
   
    mounted(){
    },
    methods:{
    },
    watch:{
        attachmentList(val){
            this.list=[];
            for(let att of val){     
                console.log(att)  
                if(att.FExt=="jpg"||att.FExt=="png"){
                     this.list.push({
                            src:'data:image/png;base64,'+att.FData,
                            msrc:'data:image/png;base64,'+att.FData,
                            w:600,
                            h:400,
                        })      
                }else if(att.FExt=='pdf'){
                    this.list.push({
                            src:'data:application/pdf;base64,'+att.FData,
                            msrc:'data:application/pdf;base64,'+att.FData,
                            w:600,
                            h:400,
                        })  
                }
            }
        }
    },
}
</script>

<style lang="scss" scoped>
    .attechment{
        padding:0.2rem;
        
        .imgList{
            .imgItem{
                height:5rem;
                margin-bottom:0.2rem;
                >img{
                    width:100%;
                    height:100%;
                }
            }
        }
    }
    

</style>
<style>

    .my-gallery img{
        height: 3rem;
        max-width:90%;
        border: 1px solid #ccc;
        padding: 0.1rem;
    }
    
</style>


